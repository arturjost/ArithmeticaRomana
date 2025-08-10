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

            // At this point we group our tokens into subtraction pairs to simplify validaiotn and calculation
            // We also validate repetition rules and subtraction rules
            List<RomanNumeralToken[]> pairs = [];
            int repetition = 0;
            for (int tokenIndex = 0; tokenIndex < numerals.Count; tokenIndex++)
            {
                RomanNumeralToken token = numerals[tokenIndex];
                RomanNumeralToken? nextToken = tokenIndex < numerals.Count - 1 ? numerals[tokenIndex + 1] : null;
                if (nextToken != null)
                {
                    if (nextToken == token)
                        repetition++;
                    else
                        repetition = 0;

                    // Tokens with BaseValue 5 can't be repeated, you can't write VV, LL or DD
                    // Other tokens with BaseValue 1 can be repeated up to three times
                    if (token.BaseValue == 5 && repetition > 0)
                        return new RomanParserResult(RomanParserError.InvalidRepetition, $"'{token.RomanNumeral}' can't be repeated.");
                    else if (token.BaseValue == 1 && repetition > 2)
                        return new RomanParserResult(RomanParserError.InvalidRepetition, $"'{token.RomanNumeral}' can only be repeated up to three times.");

                    // subtraction pair found validate if subtraction is valid
                    if (nextToken.NumeralValue > token.NumeralValue)
                    {
                        if ((token.BaseValue != 1) ||
                           (token.BaseValue == 1 && nextToken.BaseValue == 1 && nextToken.Exponent - 1 != token.Exponent) ||
                           (nextToken.BaseValue == 5 && nextToken.Exponent != token.Exponent))
                            return new RomanParserResult(RomanParserError.InvalidSubtraction, $"'{token.RomanNumeral}' can't be suptracted from {nextToken.RomanNumeral}.");

                        RomanNumeralToken[] pair = [token, nextToken];
                        pairs.Add(pair);
                        tokenIndex++;
                    }
                    else
                    {
                        pairs.Add([token]);
                    }
                }
                else
                {
                    pairs.Add([token]);
                }
            }

            // Valid Roman numeral found, calculate value
            int total = 0;
            int value = 0;
            int lastValue = 0;
            RomanNumeralToken[]? lastAdd = null;
            RomanNumeralToken[]? lastSub = null;
            foreach (var pair in pairs)
            {
                if (pair.Length == 2)
                {
                    value = pair[1].NumeralValue - pair[0].NumeralValue;
                    // You can't subtract and add the same value
                    if (lastAdd != null && (lastAdd[0] == pair[0] || pair[1] == lastAdd[0] && lastAdd[0].BaseValue == 5))
                    {
                        return new RomanParserResult(RomanParserError.InvalidSequence, $"The numeral '{pair[0].RomanNumeral}{pair[1].RomanNumeral}' cannot follow after '{lastAdd[0].RomanNumeral}'.");
                    }
                    lastSub = pair;
                }
                else
                {
                    value = pair[0].NumeralValue;
                    // Numerals with BaseValue 5 can't be repeated in the next addition
                    if (lastSub != null && (lastSub[0] == pair[0] || lastSub[1] == pair[0] && lastSub[1].BaseValue == 5))
                    {
                        return new RomanParserResult(RomanParserError.InvalidSequence, $"The numeral '{pair[0].RomanNumeral}' cannot follow after '{lastSub[0].RomanNumeral}{lastSub[1].RomanNumeral}'.");
                    }
                    lastAdd = pair;
                }
                if (value > lastValue && lastValue > 0)
                    return new RomanParserResult(RomanParserError.InvalidSequence, $"The value of each numeral must be less than or equal to the preceding one.");  
                else
                    lastValue = value;

                if(total > (int.MaxValue - value))
                    return new RomanParserResult(RomanParserError.OutOfRange, $"The number is too large to be represented.");

                total += value;
            }
            return new RomanParserResult(new RomanNumeral(total));
        }

        /// <summary>
        /// <para>This function helps us to tokenize our input.</para>
        /// <para>It also returns an out parameter, if the input could not be fully tokenized.</para>
        /// </summary>
        /// <param name="input">Input to be tokenized.</param>
        /// <param name="remainingInput">Not recognized input.</param>
        /// <returns>A list of Roman numerals ordered by position found in the input.</returns>
        private List<RomanNumeralToken> TokenizeInput(string input, out string remainingInput)
        {
            // We initalize our array with the length of the input, we can't have more tokens than that
            (RomanNumeralToken token, int position)[] tokens = new (RomanNumeralToken token, int position)[input.Length];

            // We iterate through our tokens, our map should give us out the tokens ordered by decending value
            // so we choose a greedy approach and get bigger tokens first, we might get conflicts with smaller 
            // tokens, but if we alreay placed a token at a specific position it should not be replaced
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
                        if (tokens[pos + offset].token == null)
                        {
                            tokens[pos + offset] = new(token, pos + offset);
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

            return [.. tokens.Where(token => token.token != null).OrderBy(x => x.position).Select(x => x.token)];
        }
    }
}
