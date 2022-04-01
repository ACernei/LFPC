namespace Lab3;


using System.Collections.Generic;

public class Lexer
{
    private int currentPosition; //currentChar position
    private int nextPosition;
    private readonly string input; 
    private char currentChar; 

    public Lexer(string input)
    {
        this.input = input;
    }

    public List<Token> Tokenizer()
    {
        ReadChar();
        var tokens = new List<Token>();

        Token token;
        do
        {
            token = NewToken();
            tokens.Add(token);
        } while (token.TokenType != TokenType.EOF);

        return tokens;
    }

    //Generate token based on current char value
    private Token NewToken()
    {
        Token token;

        SkipWhitespaces();

        switch (currentChar)
        {
            case '+':
                if (PeekChar() == '+')
                {
                    ReadChar(); //move one step forward
                    token = new Token(TokenType.INCREMENT, "INCREMENT");
                }
                else token = new Token(TokenType.ADDITION, "ADDITION");
                break;
            case '-':
                if (PeekChar() == '-')
                {
                    ReadChar();
                    token = new Token(TokenType.DECREMENT, "DECREMENT");
                }
                else token = new Token(TokenType.SUBTRACTION, "SUBTRACTION");
                break;
            case '*':
                token = new Token(TokenType.MULTIPLICATION, "MULTIPLICATION");
                break;
            case '/':
                token = new Token(TokenType.DIVISION, "DIVISION");
                break;
            case '!':
                if (PeekChar() == '=')
                {
                    ReadChar();
                    token = new Token(TokenType.NOT_EQUAL, "NOT_EQUAL");
                }
                else token = new Token(TokenType.NOT, "NOT");
                break;
            case '>':
                if (PeekChar() == '=')
                {
                    ReadChar();
                    token = new Token(TokenType.GREATER_OR_EQUAL, "GREATER_OR_EQUAL");
                }
                else token = new Token(TokenType.GREATER, "GREATER");
                break;
            case '<':
                if (PeekChar() == '=')
                {
                    ReadChar();
                    token = new Token(TokenType.LESS_OR_EQUAL, "LESS_OR_EQUAL");
                }
                else token = new Token(TokenType.LESS, "LESS");
                break;
            case '=':
                if (PeekChar() == '=')
                {
                    ReadChar();
                    token = new Token(TokenType.EQUAL, "EQUAL");
                }
                else token = new Token(TokenType.ASSIGN, "ASSIGN");
                break;
            case ',':
                token = new Token(TokenType.COMMA, "COMMA");
                break;
            case ';':
                token = new Token(TokenType.SEMICOLON, "SEMICOLON");
                break;
            case '(':
                token = new Token(TokenType.OPEN_PARENTHESIS, "OPEN_PARENTHESIS");
                break;
            case ')':
                token = new Token(TokenType.CLOSE_PARENTHESIS, "CLOSE_PARENTHESIS");
                break;
            case '[':
                token = new Token(TokenType.OPEN_BRACKET, "OPEN_BRACKET");
                break;
            case ']':
                token = new Token(TokenType.CLOSE_BRACKET, "CLOSE_BRACKET");
                break;
            case '{':
                token = new Token(TokenType.OPEN_BRACE, "OPEN_BRACE");
                break;
            case '}':
                token = new Token(TokenType.CLOSE_BRACE, "CLOSE_BRACE");
                break;
            case '\0':
                token = new Token(TokenType.EOF, "");
                break;
            case '"':
                ReadChar(); //skip quotation mark at the beginning of the string
                var str = ReadString();
                ReadChar(); //skip quotation mark at the end of the string

                return new Token(TokenType.STRING_VALUE, str);
                break;
            
            default:
                if (IsLetter())
                {
                    var literal = ReadIdentifier();
                    var tokenType = Token.CheckKeyword(literal);
                    return new Token(tokenType, literal);
                }
                else if (IsDigit())
                {
                    return ReadNumber();
                }
                else
                {
                    token = new Token(TokenType.ILLEGAL, currentChar.ToString());
                }
                break;
        }

        ReadChar();
        return token;
    }

    private void SkipWhitespaces()
    {
        while (currentChar is ' ' or '\t' or '\n' or '\r')
        {
            ReadChar();
        }
    }

    private void ReadChar()
    {
        //check if EOF
        //go to next char
        if (nextPosition >= input.Length) currentChar = '\0';
        else currentChar = input[nextPosition];
        currentPosition = nextPosition;
        nextPosition++;
    }

    private string ReadIdentifier()
    {
        var pos = currentPosition;
        while (IsLetter() || IsDigit())
        {
            ReadChar();
        }

        //return identifier
        return input.Substring(pos, currentPosition - pos);
    }
    
    private string ReadString()
    {
        var pos = currentPosition;
        while (currentChar != '"')
        {
            ReadChar();
        }

        //return string
        return input.Substring(pos, currentPosition - pos);
    }
  
    private Token ReadNumber()
    {
        //natural number
        var pos = currentPosition;
        while (IsDigit())
        {
            ReadChar();
        }

        //float number
        if (currentChar == '.')
        {
            ReadChar(); //skip dot
            while (IsDigit())
            {
                ReadChar();
            }
        }
        
        //return number
        return new Token(TokenType.NUMBER_VALUE, input.Substring(pos, currentPosition - pos));
    }

    private char PeekChar()
    {
        if (nextPosition > input.Length) return '\0';
        return input[nextPosition];
    }

    private bool IsLetter()
    {
        return currentChar is >= 'a' and <= 'z' or >= 'A' and <= 'Z' or '_';
    }

    private bool IsDigit()
    {
        return currentChar is >= '0' and <= '9';
    }
}
