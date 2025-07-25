/*
Yarn Spinner is licensed to you under the terms found in the file LICENSE.md.
*/

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using YarnSpinnerGodot;
using YarnAction = YarnSpinnerGodot.Action;
using System.IO;

#nullable enable



[Generator]
public class ActionRegistrationSourceGenerator : ISourceGenerator
{
    const string YarnSpinnerCompilerAssemblyName = "YarnSpinner.Compiler";
    const string DebugLoggingPreprocessorSymbol = "YARN_SOURCE_GENERATION_DEBUG_LOGGING";
    const string MinimumGodotVersionPreprocessorSymbol = "if GODOT4_0_OR_GREATER";

    public static string? GetProjectRoot(GeneratorExecutionContext context)
    {
        
        // Try and find any additional files passed to the context
        if (!context.AdditionalFiles.Any())
        {
            return null;
        }

        // One of those files is (AssemblyName).[godot]AdditionalFile.txt, and it
        // contains the path to the project
        var relevantFiles = context.AdditionalFiles.Where(
            i => i.Path.EndsWith($".godot")
        );

        if (!relevantFiles.Any())
        {
            return null;
        }

        var assemblyRelevantFile = relevantFiles.First();

        // The file needs to exist on disk
        if (!File.Exists(assemblyRelevantFile.Path))
        {
            return null;
        }

        try
        {
            // Attempt to read it - it should contain the path to the project directory
            var projectPath = File.ReadAllText(assemblyRelevantFile.Path);
            if (Directory.Exists(projectPath))
            {
                // If this directory exists, we're done
                return projectPath;
            }
            else
            {
                return null;
            }
        }
        catch (IOException)
        {
            // We encountered a problem while testing
            return null;
        }
    }


    public void Execute(GeneratorExecutionContext context)
    {
        using var output = GetOutput(context);
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();


        string projectPath = null;
        output.WriteLine(DateTime.Now);

        // Try to locate project.godot 
        if (context.AdditionalFiles.Any())
        {
            var relevants = context.AdditionalFiles.Where(i => i.Path.Contains($"{context.Compilation.AssemblyName}.AdditionalFile.txt"));
            if (relevants.Any())
            {
                var arsgacsaf = relevants.First();
#pragma warning disable RS1035
                if (File.Exists(arsgacsaf.Path))

                {
                    try
                    {
                        projectPath = arsgacsaf.Path;
                        var fullPath = Path.Combine(projectPath, "project.godot");
                        output.WriteLine($"Attempting to read settings file at {fullPath}");
                        var godotProjectText = File.ReadAllText(fullPath);
                        if (godotProjectText.Contains("disableActionsCodeRegistration"))
                        {
                            output.WriteLine("Skipping codegen due to settings.");
                            return;
                        }
                    }
                    catch (Exception e)
                    {
                        output.WriteLine($"Unable to determine Yarn settings, settings values will be ignored and codegen will occur: {e.Message}");
                    }
                }
                else
                {
                    output.WriteLine($"The project settings path metadata file does not exist at: {arsgacsaf.Path}. Settings values will be ignored and codegen will occur");
                }
            }
            else
            {
                output.WriteLine("Unable to determine Yarn settings path, no file containing the project path metadata was included. Settings values will be ignored and codegen will occur.");
            }
        }
        else
        {
            output.WriteLine("Unable to determine Yarn settings path as no additional files were included. Settings values will be ignored and codegen will occur.");
        }

        try
        {
            output.WriteLine("Source code generation for assembly " + context.Compilation.AssemblyName);

            if (context.AdditionalFiles.Any())
            {
                output.WriteLine($"Additional files:");
                foreach (var item in context.AdditionalFiles)
                {
                    output.WriteLine("  " + item.Path);
                }
            }

            output.WriteLine("Referenced assemblies for this compilation:");
            foreach (var referencedAssembly in context.Compilation.ReferencedAssemblyNames)
            {
                output.WriteLine(" - " + referencedAssembly.Name);
            }

            bool compilationReferencesYarnSpinner = context.Compilation.ReferencedAssemblyNames
                .Any(name => name.Name == YarnSpinnerCompilerAssemblyName);

            if (compilationReferencesYarnSpinner == false)
            {
                // This compilation doesn't reference YarnSpinner.Compiler. Any
                // code that we generate that references symbols in that
                // assembly won't work.
                output.WriteLine($"Assembly {context.Compilation.AssemblyName} doesn't reference {YarnSpinnerCompilerAssemblyName}. Not generating any code for it.");
                return;
            }

            output.WriteLine("Preprocessor Symbols: ");
            foreach (var symbol in context.ParseOptions.PreprocessorSymbolNames)
            {
                output.WriteLine("- " + symbol);
            }
            
            // Don't generate source code for certain Yarn Spinner provided
            // assemblies - these always manually register any actions in them.
            var prefixesToIgnore = new List<string>()
            {
                "YarnSpinnerGodot",
            };
            

            if (context.Compilation.AssemblyName == null)
            {
                output.WriteLine("Not generating registration code, because the provided AssemblyName is null");
                return;
            }

            if (prefixesToIgnore.Any(prefix => context.Compilation.AssemblyName.StartsWith(prefix)))
            {
                output.WriteLine($"Not generating registration code for {context.Compilation.AssemblyName}: we've been told to exclude it, because its name begins with one of these prefixes: {string.Join(", ", prefixesToIgnore)}");
                return;

            }

            if (!(context.Compilation is CSharpCompilation compilation))
            {
                // This is not a C# compilation, so we can't do analysis.
                output.WriteLine($"Stopping code generation because compilation is not a {nameof(CSharpCompilation)}.");
                return;
            }

            var actions = new List<YarnAction>();
            foreach (var tree in compilation.SyntaxTrees)
            {
                actions.AddRange(Analyser.GetActions(compilation, tree, output).Where(a => a.DeclarationType == DeclarationType.Attribute));
            }

            if (actions.Any() == false)
            {
                output.WriteLine($"Didn't find any Yarn Actions in {context.Compilation.AssemblyName}. Not generating any source code for it.");
                return;
            }



            HashSet<string> removals = new HashSet<string>();
            // validating and logging all the actions
            foreach (var action in actions)
            {
                if (action == null)
                {
                    output.WriteLine($"Action is null??");
                    continue;
                }

                var diagnostics = action.Validate(compilation);
                foreach (var diagnostic in diagnostics)
                {
                    context.ReportDiagnostic(diagnostic);
                    output.WriteLine($"Skipping '{action.Name}' ({action.MethodName}): {diagnostic}");
                }

                if (diagnostics.Count > 0)
                {
                    continue;
                }

                // TODO: Allow async actions (i.e. action methods that return an
                // awaitable type) once support for them is added to the actions
                // system

                output.WriteLine($"Action {action.Name}: {action.SourceFileName}:{action.Declaration?.GetLocation()?.GetLineSpan().StartLinePosition.Line} ({action.Type})");
            }

            // removing any actions that failed validation
            actions = actions.Where(x => !removals.Contains(x.Name)).ToList();

            output.Write($"Generating source code...");

            var source = Analyser.GenerateRegistrationFileSource(actions);

            output.WriteLine($"Done.");

            SourceText sourceText = SourceText.From(source, Encoding.UTF8);

            output.Write($"Writing generated source...");

            DumpGeneratedFile(context, source);

            output.WriteLine($"Done.");

            context.AddSource($"YarnActionRegistration-{compilation.AssemblyName}.Generated.cs", sourceText);
            // todo plugin settings ? 
            // if (settings != null)
            // {
            //     if (settings.generateYSLSFile)
            //     {
                    output.Write($"Generating ysls...");
                    // generating the ysls
                    YSLSGenerator generator = new YSLSGenerator(output);

                    foreach (var action in actions)
                    {
                        generator.AddAction(action);
                    }
                    var ysls = generator.Serialise();
                    output.WriteLine($"Done.");

                    if (!string.IsNullOrEmpty(projectPath))
                    {
                        output.Write($"Writing generated ysls...");

                        var fullPath = Path.Combine(projectPath, "ysls.json");
                        try
                        {
                            System.IO.File.WriteAllText(fullPath, ysls);
                            output.WriteLine($"Done.");
                        }
                        catch (Exception e)
                        {
                            output.WriteLine($"Unable to write ysls to disk: {e.Message}");
                        }
                    }
                    else
                    {
                        output.WriteLine("unable to identify project path, ysls will not be written to disk");
                    }
                // }
                // else
                // {
                //     output.WriteLine($"skipping ysls generation due to settings");
                // }
            // }
            // else
            // {
            //     output.WriteLine($"skipping ysls generation due to settings not being found");
            // }

            stopwatch.Stop();
            output.WriteLine($"Source code generation completed in {stopwatch.Elapsed.TotalMilliseconds}ms");
            return;

        }
        catch (Exception e)
        {
            output.WriteLine($"{e}");
        }
    }

    private MethodDeclarationSyntax GenerateLoggingMethod(string methodName, string sourceExpression, string prefix)
    {
        return SyntaxFactory.MethodDeclaration(
    SyntaxFactory.PredefinedType(
        SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
    SyntaxFactory.Identifier(methodName))
.WithModifiers(
    SyntaxFactory.TokenList(
        new[]{
            SyntaxFactory.Token(SyntaxKind.PublicKeyword),
            SyntaxFactory.Token(SyntaxKind.StaticKeyword)}))
.WithBody(
    SyntaxFactory.Block(
        SyntaxFactory.LocalDeclarationStatement(
            SyntaxFactory.VariableDeclaration(
                SyntaxFactory.GenericName(
                    SyntaxFactory.Identifier("IEnumerable"))
                .WithTypeArgumentList(
                    SyntaxFactory.TypeArgumentList(
                        SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                            SyntaxFactory.PredefinedType(
                                SyntaxFactory.Token(SyntaxKind.StringKeyword))))))
            .WithVariables(
                SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                    SyntaxFactory.VariableDeclarator(
                        SyntaxFactory.Identifier("source"))
                    .WithInitializer(
                        SyntaxFactory.EqualsValueClause(
                            SyntaxFactory.ParseExpression(sourceExpression)))))),
        SyntaxFactory.LocalDeclarationStatement(
            SyntaxFactory.VariableDeclaration(
                SyntaxFactory.IdentifierName(
                    SyntaxFactory.Identifier(
                        SyntaxFactory.TriviaList(),
                        SyntaxKind.VarKeyword,
                        "var",
                        "var",
                        SyntaxFactory.TriviaList())))
            .WithVariables(
                SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                    SyntaxFactory.VariableDeclarator(
                        SyntaxFactory.Identifier("prefix"))
                    .WithInitializer(
                        SyntaxFactory.EqualsValueClause(
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.StringLiteralExpression,
                                SyntaxFactory.Literal(prefix))))))),
        SyntaxFactory.ExpressionStatement(
            SyntaxFactory.InvocationExpression(
                SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.IdentifierName("Debug"),
                    SyntaxFactory.IdentifierName("Log")
                )
            )
            .WithArgumentList(
                SyntaxFactory.ArgumentList(
                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                        SyntaxFactory.Argument(
                            SyntaxFactory.InterpolatedStringExpression(
                                SyntaxFactory.Token(SyntaxKind.InterpolatedVerbatimStringStartToken)
                            )
                            .WithContents(
                                SyntaxFactory.List<InterpolatedStringContentSyntax>(
                                    new InterpolatedStringContentSyntax[]{
                                        SyntaxFactory.Interpolation(
                                            SyntaxFactory.IdentifierName("prefix")
                                        ),
                                        SyntaxFactory.InterpolatedStringText()
                                        .WithTextToken(
                                            SyntaxFactory.Token(
                                                SyntaxFactory.TriviaList(),
                                                SyntaxKind.InterpolatedStringTextToken,
                                                " ",
                                                " ",
                                                SyntaxFactory.TriviaList()
                                            )
                                        ),
                                        SyntaxFactory.Interpolation(
                                            SyntaxFactory.InvocationExpression(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.PredefinedType(
                                                        SyntaxFactory.Token(SyntaxKind.StringKeyword)
                                                    ),
                                                    SyntaxFactory.IdentifierName("Join")
                                                )
                                            )
                                            .WithArgumentList(
                                                SyntaxFactory.ArgumentList(
                                                    SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                                        new SyntaxNodeOrToken[]{
                                                            SyntaxFactory.Argument(
                                                                SyntaxFactory.LiteralExpression(
                                                                    SyntaxKind.CharacterLiteralExpression,
                                                                    SyntaxFactory.Literal(';')
                                                                )
                                                            ),
                                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                            SyntaxFactory.Argument(
                                                                SyntaxFactory.IdentifierName("source")
                                                            )
                                                        }
                                                    )
                                                )
                                            )
                                        )
                                    }
                                )
                            )
                        )
                    )
                )
            )
        )
    )
)
.NormalizeWhitespace();
    }

    public static MethodDeclarationSyntax GenerateSingleLogMethod(string methodName, string text, string prefix)
    {
        return SyntaxFactory.MethodDeclaration(
            SyntaxFactory.PredefinedType(
                SyntaxFactory.Token(SyntaxKind.VoidKeyword)
            ),
            SyntaxFactory.Identifier(methodName)
        )
        .WithModifiers(
            SyntaxFactory.TokenList(
                new[]{
                    SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                    SyntaxFactory.Token(SyntaxKind.StaticKeyword)
                }
            )
        )
        .WithBody(
            SyntaxFactory.Block(
                SyntaxFactory.SingletonList<StatementSyntax>(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("Debug"),
                                SyntaxFactory.IdentifierName("Log")
                            )
                        )
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.InterpolatedStringExpression(
                                            SyntaxFactory.Token(SyntaxKind.InterpolatedStringStartToken)
                                        )
                                        .WithContents(
                                            SyntaxFactory.List<InterpolatedStringContentSyntax>(
                                                new InterpolatedStringContentSyntax[]{
                                                    SyntaxFactory.Interpolation(
                                                        SyntaxFactory.LiteralExpression(
                                                            SyntaxKind.StringLiteralExpression,
                                                            SyntaxFactory.Literal(prefix)
                                                        )
                                                    ),
                                                    SyntaxFactory.InterpolatedStringText()
                                                    .WithTextToken(
                                                        SyntaxFactory.Token(
                                                            SyntaxFactory.TriviaList(),
                                                            SyntaxKind.InterpolatedStringTextToken,
                                                            " ",
                                                            " ",
                                                            SyntaxFactory.TriviaList()
                                                        )
                                                    ),
                                                    SyntaxFactory.Interpolation(
                                                        SyntaxFactory.LiteralExpression(
                                                            SyntaxKind.StringLiteralExpression,
                                                            SyntaxFactory.Literal(text)
                                                        )
                                                    )
                                                }
                                            )
                                        )
                                    )
                                )
                            )
                        )
                    )
                )
            )
        )
        .NormalizeWhitespace();
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new ClassDeclarationSyntaxReceiver());
    }

    static string GetTemporaryPath(GeneratorExecutionContext context)
    {
        string tempPath;
        var rootPath = GetProjectRoot(context);
        if (rootPath != null)
        {
            tempPath = Path.Combine(rootPath, "Logs",  "yarn_spinner_godot");
        }
        else
        {
            tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "dev.yarnspinner.logs");
        }

        // we need to make the logs folder, but this can potentially fail
        // if it does fail then we will just chuck the logs inside the tmp folder
        try
        {
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }
        }
        catch
        {
            tempPath = System.IO.Path.GetTempPath();
        }
        return tempPath;
    }

    public YarnSpinnerGodot.ILogger GetOutput(GeneratorExecutionContext context)
    {
        if (GetShouldLogToFile(context))
        {
            var tempPath = ActionRegistrationSourceGenerator.GetTemporaryPath(context);

            var path = System.IO.Path.Combine(tempPath, $"{nameof(ActionRegistrationSourceGenerator)}-{context.Compilation.AssemblyName}.txt");
            var outFile = System.IO.File.Open(path, System.IO.FileMode.Create);

            return new YarnSpinnerGodot.FileLogger(new System.IO.StreamWriter(outFile));
        }
        else
        {
            return new YarnSpinnerGodot.NullLogger();
        }
    }

    private static bool GetShouldLogToFile(GeneratorExecutionContext context)
    {
        return context.ParseOptions.PreprocessorSymbolNames.Contains(DebugLoggingPreprocessorSymbol);
    }

    public void DumpGeneratedFile(GeneratorExecutionContext context, string text)
    {
        if (GetShouldLogToFile(context))
        {
            var tempPath = ActionRegistrationSourceGenerator.GetTemporaryPath(context);
            var path = System.IO.Path.Combine(tempPath, $"{nameof(ActionRegistrationSourceGenerator)}-{context.Compilation.AssemblyName}.cs");
            System.IO.File.WriteAllText(path, text);
        }
    }
}

internal class ClassDeclarationSyntaxReceiver : ISyntaxReceiver
{
    public List<ClassDeclarationSyntax> Classes { get; private set; } = new List<ClassDeclarationSyntax>();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        // Business logic to decide what we're interested in goes here
        if (syntaxNode is ClassDeclarationSyntax cds)
        {
            Classes.Add(cds);
        }
    }
}

internal class YSLSGenerator
{
    public YSLSGenerator(YarnSpinnerGodot.ILogger logger)
    {
        this.logger = logger;
    }
    struct YarnActionParameter
    {
        internal string Name;
        internal string Type;
        internal bool IsParamsArray;

        internal Dictionary<string, object> ToDictionary()
        {
            var dict = new Dictionary<string, object>();
            dict["Name"] = Name;
            dict["Type"] = Type;
            dict["IsParamsArray"] = IsParamsArray;
            return dict;
        }
    }
    struct YarnActionCommand
    {
        internal string YarnName;
        internal string DefinitionName;
        internal string Signature;
        internal string FileName;
        internal YarnActionParameter[] Parameters;

        internal Dictionary<string, object> ToDictionary()
        {
            var dict = new Dictionary<string, object>();
            dict["YarnName"] = YarnName;
            dict["DefinitionName"] = DefinitionName;
            dict["Signature"] = Signature;
            dict["Language"] = "csharp";

            if (!string.IsNullOrEmpty(FileName))
            {
                dict["FileName"] = FileName;
            }

            if (Parameters.Length > 0)
            {
                var pl = new List<Dictionary<string, object>>();
                foreach (var p in Parameters)
                {
                    pl.Add(p.ToDictionary());
                }
                dict["Parameters"] = pl;
            }
            return dict;
        }
    }
    struct YarnActionFunction
    {
        internal string YarnName;
        internal string DefinitionName;
        internal string Signature;
        internal YarnActionParameter[] Parameters;
        internal string ReturnType;
        internal string FileName;

        internal Dictionary<string, object> ToDictionary()
        {
            var dict = new Dictionary<string, object>();
            dict["YarnName"] = YarnName;
            dict["DefinitionName"] = DefinitionName;
            dict["Signature"] = Signature;
            dict["ReturnType"] = ReturnType;
            dict["Language"] = "csharp";

            if (!string.IsNullOrEmpty(FileName))
            {
                dict["FileName"] = FileName;
            }

            if (Parameters.Length > 0)
            {
                var pl = new List<Dictionary<string, object>>();
                foreach (var p in Parameters)
                {
                    pl.Add(p.ToDictionary());
                }
                dict["Parameters"] = pl;
            }
            return dict;
        }
    }

    internal YarnSpinnerGodot.ILogger logger;
    List<YarnActionCommand> commands = new List<YarnActionCommand>();
    List<YarnActionFunction> functions = new List<YarnActionFunction>();

    internal string Serialise()
    {
        var commandLine = "\"Commands\":[]";
        var functionLine = "\"Functions\":[]";
        // do we have any commands?
        if (commands.Count() > 0)
        {
            var result = string.Join(",", commands.Select(c => YarnSpinnerGodot.Json.Serialize(c.ToDictionary())));
            commandLine = $"\"Commands\":[{result}]";
        }
        // do we have any functions?
        if (functions.Count() > 0)
        {
            var result = string.Join(",", functions.Select(f => YarnSpinnerGodot.Json.Serialize(f.ToDictionary())));
            functionLine = $"\"Functions\":[{result}]";
        }
        return $"{{{commandLine},{functionLine}}}";
    }
    internal void AddAction(YarnAction action)
    {
        switch (action.Type)
        {
            case ActionType.Command:
                AddCommand(action);
                break;
            case ActionType.Function:
                AddFunction(action);
                break;
            case ActionType.NotAnAction:
                logger.WriteLine($"attempted to make a ysls string for {action.Name}, but it is not a command or function");
                break;
        }
    }
    private void AddFunction(YarnAction action)
    {
        var parameters = GenerateParams(action.Parameters);
        var Signature = $"{action.Name}({string.Join(", ", parameters.Select(p => p.Name))})";
        if (parameters.Length == 0)
        {
            Signature = $"{action.Name}()";
        }
        var function = new YarnActionFunction
        {
            YarnName = action.Name,
            DefinitionName = action.MethodIdentifierName,
            Signature = Signature,
            Parameters = parameters,
            ReturnType = InternalTypeToYarnType(action.MethodSymbol.ReturnType),
            FileName = action.SourceFileName,
        };
        functions.Add(function);
    }
    private void AddCommand(YarnAction action)
    {
        var parameters = GenerateParams(action.Parameters);
        var Signature = $"<<{action.Name} {string.Join(" ", parameters.Select(p => p.Name))}>>";
        if (parameters.Length == 0)
        {
            Signature = $"<<{action.Name}>>";
        }
        var command = new YarnActionCommand
        {
            YarnName = action.Name,
            DefinitionName = action.MethodIdentifierName,
            Signature = Signature,
            Parameters = parameters,
            FileName = action.SourceFileName,
        };
        commands.Add(command);
    }
    private YarnActionParameter[] GenerateParams(List<Parameter> parameters)
    {
        List<YarnActionParameter> parameterList = new List<YarnActionParameter>();
        foreach (var param in parameters)
        {
            var paramType = InternalTypeToYarnType(param.Type);

            var parameter = new YarnActionParameter
            {
                Name = param.Name,
                Type = paramType,
                IsParamsArray = false,
            };
            parameterList.Add(parameter);
        }
        return parameterList.ToArray();
    }
    private string InternalTypeToYarnType(ITypeSymbol symbol)
    {
        var type = "any";
        switch (symbol.SpecialType)
        {
            case SpecialType.System_Boolean:
                type = "boolean";
                break;
            case SpecialType.System_SByte:
            case SpecialType.System_Byte:
            case SpecialType.System_Int16:
            case SpecialType.System_UInt16:
            case SpecialType.System_Int32:
            case SpecialType.System_UInt32:
            case SpecialType.System_Int64:
            case SpecialType.System_UInt64:
            case SpecialType.System_Decimal:
            case SpecialType.System_Single:
            case SpecialType.System_Double:
                type = "number";
                break;
            case SpecialType.System_String:
                type = "string";
                break;
        }
        return type;
    }
}
