/*
Yarn Spinner is licensed to you under the terms found in the file LICENSE.md.
*/

using System.Collections.Generic;
using System.Threading;
using Godot;
using Yarn.Markup;

#nullable enable

namespace YarnSpinnerGodot;

/// <summary>
/// Allows pausing the current typewriter through [pause/] markers.
/// </summary>
public sealed class PauseEventProcessor : IActionMarkupHandler
{
    private Dictionary<int, float> pauses = new();

    public void OnLineDisplayComplete()
    {
        pauses.Clear();
    }

    public void OnLineDisplayBegin(MarkupParseResult line, RichTextLabel text)
    {
        pauses = new();
        // grabbing out any pauses inside the line
        foreach (var attribute in line.Attributes)
        {
            if (attribute.Name != "pause")
            {
                continue;
            }

            if (attribute.Properties.TryGetValue("pause", out MarkupValue value))
            {
                // depending on the property value we need to take a different path this is because they have made it an integer or a float which are roughly the same.
                // But they also might have done something weird and we need to handle that
                switch (value.Type)
                {
                    case MarkupValueType.Integer:
                        pauses.Add(attribute.Position, value.IntegerValue);
                        break;
                    case MarkupValueType.Float:
                        pauses.Add(attribute.Position, value.FloatValue * 1000);
                        break;
                    default:
                        GD.PushWarning(
                            $"Pause property is of type {value.Type}, which is not allowed. Defaulting to one second.");
                        pauses.Add(attribute.Position, 1000);

                        break;
                }
            }
            else
            {
                // they haven't set a duration, so we will instead use the
                // default of one second
                pauses.Add(attribute.Position, 1000);
            }
        }
    }

    public void OnPrepareForLine(MarkupParseResult line, RichTextLabel text)
    {
    }

    public async YarnTask OnCharacterWillAppear(int currentCharacterIndex, MarkupParseResult line,
        CancellationToken cancellationToken)
    {
        if (pauses.TryGetValue(currentCharacterIndex, out var duration))
        {
            await YarnTask.Delay(System.TimeSpan.FromMilliseconds(duration), cancellationToken)
                .SuppressCancellationThrow();
        }
    }

    public void OnLineWillDismiss()
    {
    }
}