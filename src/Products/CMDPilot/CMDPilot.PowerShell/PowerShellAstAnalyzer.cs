using System.Management.Automation.Language;

namespace Company.CMDPilot.PowerShell;

/// <summary>
/// Provides capabilities to parse and analyze PowerShell scripts using its native AST.
/// </summary>
public static class PowerShellAstAnalyzer
{
    /// <summary>
    /// Analyzes the provided script text and extracts the names of commands invoked.
    /// </summary>
    /// <param name="scriptText">The PowerShell script text.</param>
    /// <returns>A list of command names found in the script.</returns>
    public static IReadOnlyList<string> ExtractCommands(string scriptText)
    {
        ScriptBlockAst ast = Parser.ParseInput(scriptText, out _, out _);

        // Find all CommandAst nodes (e.g., Get-Process, Set-Item)
        IEnumerable<Ast> commandAsts = ast.FindAll(node => node is CommandAst, searchNestedScriptBlocks: true);

        List<string> commands = [];
        foreach (CommandAst cmd in commandAsts.Cast<CommandAst>())
        {
            string commandName = cmd.GetCommandName();
            if (!string.IsNullOrWhiteSpace(commandName))
            {
                commands.Add(commandName);
            }
        }

        return commands;
    }

    /// <summary>
    /// Detects potentially obfuscated or dangerous syntax, such as dynamic invocation ('&amp;' operator with variables).
    /// </summary>
    /// <param name="scriptText">The PowerShell script text.</param>
    /// <returns>True if obfuscation patterns are detected; otherwise, false.</returns>
    public static bool DetectObfuscation(string scriptText)
    {
        ScriptBlockAst ast = Parser.ParseInput(scriptText, out _, out _);

        // Check for Invoke-Expression
        IEnumerable<Ast> invokeExpressions = ast.FindAll(node =>
        {
            if (node is CommandAst cmd)
            {
                string name = cmd.GetCommandName();
                if (name != null && name.Equals("Invoke-Expression", System.StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                if (name != null && name.Equals("iex", System.StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }, searchNestedScriptBlocks: true);

        if (invokeExpressions.Any())
        {
            return true;
        }

        // Check for dynamic invocation operator '&amp;' where the command is not a simple string
        IEnumerable<Ast> dynamicInvocations = ast.FindAll(node =>
        {
            if (node is CommandAst cmd && cmd.InvocationOperator == TokenKind.Ampersand)
            {
                if (cmd.CommandElements.Count > 0 && cmd.CommandElements[0] is not StringConstantExpressionAst)
                {
                    return true;
                }
            }
            return false;
        }, searchNestedScriptBlocks: true);

        return dynamicInvocations.Any();
    }
}
