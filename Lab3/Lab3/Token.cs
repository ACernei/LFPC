namespace Lab3;

using System.Collections.Generic;

public class Token
{
    private static readonly Dictionary<string, TokenType> predefinedKeywords = new()
    {
        ["START"] = TokenType.START,
        ["FINISH"] = TokenType.FINISH,
        ["function"] = TokenType.FUNCTION,
        ["Main"] = TokenType.MAIN,
        ["var"] = TokenType.VAR,
        ["true"] = TokenType.TRUE,
        ["false"] = TokenType.FALSE,
        ["null"] = TokenType.NULL,
        ["return"] = TokenType.RETURN,
        ["message"] = TokenType.MESSAGE,
        ["input"] = TokenType.INPUT,
        ["output"] = TokenType.OUTPUT,
        ["if"] = TokenType.IF,
        ["else"] = TokenType.ELSE,
        ["elif"] = TokenType.ELIF,
        ["pow"] = TokenType.POW,
    };

    private static readonly List<TokenType> valueTypes = new()
    {
        TokenType.IDENTIFIER,
        TokenType.NUMBER_VALUE,
        TokenType.STRING_VALUE,
    };
    
    public Token(TokenType tokenType, string value)
    {
        TokenType = tokenType;
        Value = value;
    }

    public TokenType TokenType { get; }
    public string Value { get; }

    public static TokenType CheckKeyword(string keyword)
    {
        return predefinedKeywords.ContainsKey(keyword) ? predefinedKeywords[keyword] : TokenType.IDENTIFIER;
    }

    public override string ToString()
    {
        if (valueTypes.Contains(TokenType))
        {
            return $"{TokenType}({Value})";
        }
        return TokenType.ToString();
    }
}
