using ArithmeticaRomana.Core.Internal;

namespace ArithmeticaRomana.Core.Parser
{
    /// <summary>
    /// The default implementation of the <see cref="IRomanNumeralParser"/> 
    /// </summary>
    public class VinculumRomanNumeralParser : IRomanNumeralParser
    {
        public IRomanParserResult Parse(string romanRepresentation)
        {
            if (string.IsNullOrWhiteSpace(romanRepresentation))
                return new RomanParserResult(RomanParserError.MalformedInput, "The input string is null or empty and can't be parsed.");

            // Get the roman numeral tokens from the input, should there be any remaining characters that means
            // the input is malformed and can't be parsed
            var numerals = TokenizeInput(romanRepresentation, out string remainigInput);
            if (numerals.Count == 0 || !string.IsNullOrWhiteSpace(remainigInput))
            {
                return new RomanParserResult(RomanParserError.MalformedInput, $"'{remainigInput}' can't be parsed.");
            }

            // If rules are not valid, we return the error
            if (!ValidateTokens(numerals, out IRomanParserResult? result))
            {
                return result!;
            }

            // If we reach this point, we can calculate the total value of the roman numeral
            int total = 0;
            numerals.Reverse();
            RomanNumeralToken? previousToken = null;
            foreach(var currentToken in numerals)
            {
                if(previousToken?.NumeralValue > currentToken.NumeralValue)
                {
                    total -= currentToken.NumeralValue;
                }
                else
                {
                    total += currentToken.NumeralValue;
                }
                previousToken = currentToken;
            }
            return new RomanParserResult(new RomanNumeral(total));
        }

        /// <summary>
        /// <para>This function helps us to tokenize our input, so we can validate afterwards.</para>
        /// <para>It also returns an out parameter, if the input could not be fully tokenized.</para>
        /// </summary>
        /// <param name="input">Input to be tokenized</param>
        /// <param name="remainingInput"></param>
        /// <returns>A list of Roman numerals ordered by position found in the input</returns>
        private List<RomanNumeralToken> TokenizeInput(string input, out string remainingInput)
        {
            // We initalize our array with the length of the input, we can't have more tokens than that
            (RomanNumeralToken token, int position)[] tokens = new (RomanNumeralToken token, int position)[input.Length];

            // We iterate through our tokens, our map should give us out the tokens ordered by decending value
            // so we choose a greedy approach and get bigger tokens first, we might get conflicts with smaller 
            // tokens, but if we alreay placed a token at a specific position it should not be replaced
            int arrayPos = 0;
            int pos = -1;
            remainingInput = input;
            foreach (var token in Internals.Vinculum.BaseTokensByValue())
            {
                // We initalize the input as the currentSpan foreach new interation
                // We also need the RomanNumeral string as a span for comparison example: M, IV, etc.. 
                // An offset is requiered, because we want to remove parts from the span
                ReadOnlySpan<char> currrentSpan = input.AsSpan();
                var numeralSpan = token.RomanNumeral.AsSpan();
                int offset = 0;
                do
                {
                    // does our currrentSpan have the numeral? Let's assume we search for "V" 
                    pos = currrentSpan.IndexOf(numeralSpan);

                    // great we found our token "V", that means we should add it to our token list
                    // the offset will be our currently offset + pos + length
                    if (pos > -1)
                    {
                        if (tokens[arrayPos].token == null)
                        {
                            tokens[arrayPos] = new(token, pos + offset);
                            arrayPos++;
                        }
                        offset += pos + numeralSpan.Length;

                        // the first characters are our token, so we remove that
                        if (pos == 0)
                        {
                            currrentSpan = currrentSpan.Slice(numeralSpan.Length, currrentSpan.Length - numeralSpan.Length);
                        }
                        // the last characters are our token, so we that means that we can finish
                        // searching for the current token
                        else if (pos + numeralSpan.Length == currrentSpan.Length)
                        {
                            break;
                        }
                        // our characters are in the middle of our input, so we can remove the first part 
                        else
                        {
                            currrentSpan = currrentSpan.Slice(pos + numeralSpan.Length, (currrentSpan.Length - numeralSpan.Length) - pos);
                        }
                    }
                } while (pos > -1 && currrentSpan.Length > 0);

                // we can just replace all apperance of the token in the remainingInput
                remainingInput = remainingInput.Replace(token.RomanNumeral, "");
                if (remainingInput.Length <= 0)
                    break;
            }

            return [.. tokens.OrderBy(x => x.position).Select(x => x.token)];
        }

        /// <summary>
        /// This function checks a given token list for subtraction and repetition rules
        /// </summary>
        /// <param name="tokens">The list of tokens to check</param>
        /// <param name="error">If validation fails; provides detailed error message.</param>
        /// <returns>if validation succeeded returns true; otherwise false</returns>
        private bool ValidateTokens(List<RomanNumeralToken> tokens, out IRomanParserResult? error)
        {
            RomanNumeralToken currentToken;
            RomanNumeralToken? nextToken = null;
            RomanNumeralToken? lookaheadToken = null;

            int repetitionCount = 0;
            for (int i = 0; i < tokens.Count; i++)
            {
                currentToken = tokens[i];
                nextToken = i < tokens.Count - 1 ? tokens[i + 1] : null;
                lookaheadToken = i < tokens.Count - 2 ? tokens[i + 2] : null;
                // if the current token value is smaller than the nextToken value, that means we are in a subtraction
                // scenario, so we need to check if the subtraction rules are taken into account 
                if (nextToken != null && nextToken.NumeralValue > currentToken.NumeralValue)
                {
                    // Only I (1), X (10), C (100) etc. can be subtracted, so only
                    // tokens with BaseValue 1 can be subtracted from the next token 
                    if (currentToken.BaseValue != 1)
                    {
                        error = new RomanParserResult(RomanParserError.InvalidSubtraction, $"'{currentToken.RomanNumeral}' can't be subtracted from '{nextToken.RomanNumeral}'.");
                        return false;
                    }

                    // Check for valid subtraction pairs.
                    // I can only subtract from V or X 
                    // X can only subtract from L or C 
                    // C can only subtract from D or M 
                    // We can only subtract if the next token is Base 1 or Base 5
                    // Valid: I (Exponent: 0) X (Exponent: 1), X (Exponent: 1) C (Exponent: 2)
                    // Invalid: I (Exponent: 0) C (Exponent: 2), C (Exponent: 2) I (Exponent: 0)
                    if (nextToken.BaseValue == 1)
                    {
                        if (currentToken.Exponent != nextToken.Exponent - 1)
                        {
                            error = new RomanParserResult(RomanParserError.InvalidSubtraction, $"'{currentToken.RomanNumeral}' can't be subtracted from '{nextToken.RomanNumeral}'.");
                            return false;
                        }
                    }
                    else if (nextToken.BaseValue == 5)
                    {
                        if (currentToken.Exponent != nextToken.Exponent)
                        {
                            error = new RomanParserResult(RomanParserError.InvalidSubtraction, $"'{currentToken.RomanNumeral}' can't be subtracted from '{nextToken.RomanNumeral}'.");
                            return false;
                        }

                        // V, L, D can't be placed after a subtraction
                        if (lookaheadToken?.RomanNumeral == currentToken.RomanNumeral)
                        {
                            error = new RomanParserResult(RomanParserError.InvalidSubtraction, $"'{nextToken.RomanNumeral}' can't be placed after '{currentToken.RomanNumeral}{lookaheadToken.RomanNumeral}'.");
                            return false;
                        }
                    }             
                }

                // We also need to check if the lookaheadToken is bigger than our currentToken, bigger values can't follow
                if (lookaheadToken?.NumeralValue > currentToken.NumeralValue)
                {
                    error = new RomanParserResult(RomanParserError.MalformedInput, $"'{lookaheadToken.RomanNumeral}' can't follow after '{currentToken.RomanNumeral}{nextToken?.RomanNumeral}'.");
                    return false;
                }

                // If the current token is the same as the previous token, we need to check the repetition rules
                if (currentToken.RomanNumeral == nextToken?.RomanNumeral)
                {
                    repetitionCount++;
                    if (currentToken.BaseValue == 5)
                    {
                        error = new RomanParserResult(RomanParserError.InvalidRepetition, $"'{currentToken.RomanNumeral}' can't be repeated.");
                        return false; // V, L and D can not be repeated
                    }
                    else if (repetitionCount > 2)
                    {
                        error = new RomanParserResult(RomanParserError.InvalidRepetition, $"'{currentToken.RomanNumeral}' can only be repeated up to three times.");
                        return false; // I, X, C and M can be used up to 3 times
                    }
                }
                else
                {
                    repetitionCount = 0;
                }
            }
            error = null;
            return true;
        }
    }
}
