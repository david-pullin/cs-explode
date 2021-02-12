using System;
using System.Text;
using System.Collections.Generic;

#nullable enable

namespace Sprocket.Text.Explode
{


    /// <summary>
    /// Details of delimiters to be used to split strings.
    /// </summary>
    public class Delimiters
    {
        /// <summary>
        /// The string, single or multiple character used as the field delimiter. Default = <c>,</c>.
        /// </summary>
        private string _fieldDelimiter = ",";

        /// <summary>
        /// The string, single or multiple character used as the text delimiter. Default = <c>"</c>
        /// </summary>
        private string _textDelimiter = "\"";

        /// <summary>
        /// The first character of property <c>FieldDelimiter</c>.  
        /// </summary>
        internal char FirstFieldDelimiterChar;

        /// <summary>
        /// The first character of property <c>TextDelimiter</c> or the null character <c>\0</c> uf TextDelimiter is an empty string.
        /// </summary>
        internal char FirstTextDelimiterChar;

        /// <summary>
        /// Set to true if the <c>FieldDelimiter</c> is a single character.
        /// </summary>
        internal bool IsFieldDelimiterASingleChar;

        /// <summary>
        /// Set to true if <c>TextDelimiter</c> is a single character.
        /// </summary>
        internal bool IsTextDelimiterASingleChar;

        /// <summary>
        /// Number of characters in <c>FieldDelimiter</c>
        /// </summary>
        internal int FieldDelimiterLength;

        /// <summary>
        /// Number of characters in <c>TextDelimiter</c>
        /// </summary>
        internal int TextDelimiterLength;

        /// <summary>
        /// Create instance of class with default values of <code>,</code> for <see cref="FieldDelimiter"/> and <c>"</c> <see cref="TextDelimiter"/>.
        /// </summary>
        public Delimiters()
        {
            this.CalculatePrivateProperties();
        }

        /// <summary>
        /// Overloaded Constructor
        /// </summary>
        /// <param name="fieldDelimiter">Character to be used for <see cref="FieldDelimiter"/>.</param>
        public Delimiters(char fieldDelimiter)
        {
            this._fieldDelimiter = fieldDelimiter.ToString();
            this.CalculatePrivateProperties();
        }

        /// <param name="fieldDelimiter">Character to be used for <see cref="FieldDelimiter"/>.</param>
        /// <param name="textDelimiter">Character to be used for <see cref="TextDelimiter"/>.</param>
        public Delimiters(char fieldDelimiter, char textDelimiter)
        {
            this._fieldDelimiter = fieldDelimiter.ToString();
            this._textDelimiter = textDelimiter.ToString();
            this.CalculatePrivateProperties();
        }

        /// <param name="fieldDelimiter">Character to be used for <see cref="FieldDelimiter"/>.</param>
        /// <param name="textDelimiter">String to be used for <see cref="TextDelimiter"/>.</param>
        public Delimiters(char fieldDelimiter, string textDelimiter)
        {
            this._fieldDelimiter = fieldDelimiter.ToString();
            this._textDelimiter = textDelimiter;
            this.CalculatePrivateProperties();
        }

        /// <param name="fieldDelimiter">String to be used for <see cref="FieldDelimiter"/>.</param>
        public Delimiters(string fieldDelimiter)
        {
            this._fieldDelimiter = fieldDelimiter;
            this.CalculatePrivateProperties();
        }

        /// <param name="fieldDelimiter">String to be used for <see cref="FieldDelimiter"/>.</param>
        /// <param name="textDelimiter">String to be used for <see cref="TextDelimiter"/>.</param>
        public Delimiters(string fieldDelimiter, string textDelimiter)
        {
            this._fieldDelimiter = fieldDelimiter;
            this._textDelimiter = textDelimiter;
            this.CalculatePrivateProperties();
        }

        /// <summary>
        /// Called from class <c>Delimiters</c> construtor to pre-calculate values for <see cref="Explode"/>.<see cref="ExplodeLine"/>.
        /// </summary>
        private void CalculatePrivateProperties()
        {
            this.FieldDelimiterLength = FieldDelimiter.Length;
            this.TextDelimiterLength = TextDelimiter.Length;
            this.IsFieldDelimiterASingleChar = (1 == FieldDelimiter.Length);
            this.IsTextDelimiterASingleChar = (1 == TextDelimiterLength);
            this.FirstFieldDelimiterChar = this._fieldDelimiter[0];
            this.FirstTextDelimiterChar = ("" != this._textDelimiter) ? this._textDelimiter[0] : '\0';
        }

        /// <summary>
        /// Which character is being used as the field delimiter.
        /// </summary>
        /// <value>Single or multiple character string. Default = <c>,</c></value>
        public string FieldDelimiter
        {
            get { return this._fieldDelimiter; }
            set
            {
                this._fieldDelimiter = value;
                this.CalculatePrivateProperties();
            }
        }

        /// <summary>
        /// Which character is being used as the text delimiter.
        /// Text delimiters are used to enclose fields that contain a Field Delimiter as content.
        /// </summary>
        /// <value>Single or multiple character string. Default = <c>"</c></value>
        public string TextDelimiter
        {
            get { return this._textDelimiter; }
            set
            {
                this._textDelimiter = value;
                this.CalculatePrivateProperties();

            }
        }
    }

    /// <summary>
    /// Represents an single field and its contents.
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// The cell's content.
        /// </summary>
        /// <value>The cells content</value>
        public string Text { get; private set; }

        /// <summary>
        /// Raw, unmodified, version of the cell's content including any Text Delimiters.
        /// </summary>
        /// <value>Cell's full unmodified contents</value>
        public string RawText { get; private set; }

        /// <summary>
        /// Was the cell's <see cref="Text"/> Text Delimited.
        /// </summary>
        /// <remarks>
        /// <div class='NOTE alert alert-warning'><h5>Warning</h5>
        /// <p>This property will not be set if <see cref="ExplodeOptions"/>.<see cref="ExplodeOptions.EnableTextDelimiter"/> was disabled.</p>
        /// </div>
        /// </remarks>
        /// <value>True if cell's <see cref="Text"/> was Text Delimited.</value>
        public bool WasTextDelimited { get; private set; }

        /// <summary>
        /// Does the cell's <see cref="Text"/> contain any Text Delimiters that were processed within a text delimited string.
        /// </summary>
        /// <remarks>
        /// <p>Given a string processed with default delimiters: <c>a,"hello ""World""",b</c></p>
        /// <p>The second cell's <see cref="Text"/> property will be set to <c>Hello "World"</c> and <see cref="ContainsTextDelimiter"/> will be set to true.</p>
        /// <div class='NOTE alert alert-note'><h5>Note</h5>
        /// <p>This property will not be set if <see cref="ExplodeOptions"/>.<see cref="ExplodeOptions.EnableTextDelimiter"/> or <see cref="ExplodeOptions"/>.<see cref="ExplodeOptions.AllowInsideDoubleTextDelimiters"/> were disabled.</p>
        /// </div>
        /// </remarks>
        /// <value>True if <see cref="Text"/> contains embedded text delimiters.</value>
        public bool ContainsTextDelimiter { get; private set; }

        /// <summary>
        /// Constructor for class <c>Cell</c>
        /// </summary>
        /// <param name="text">Processed text</param>
        /// <param name="rawText">Original text containing any Text Delimters, Leading or Trailing Spaces.</param>
        /// <param name="wasTextDelimited">Set to true if <c>text</c> was Text Delimited></param>
        /// <param name="containsTextDelimiter">Set to true if <c>text</c> contains a Text Delimiter as content.</param>
        internal Cell(StringBuilder text, StringBuilder rawText, bool wasTextDelimited, bool containsTextDelimiter)
        {
            this.Text = text.ToString();
            this.RawText = rawText.ToString();
            this.WasTextDelimited = wasTextDelimited;
            this.ContainsTextDelimiter = containsTextDelimiter;
        }
    }

    /// <summary>
    /// A collection of <see cref="Cell" /> objects.
    /// </summary>
    public class Cells : List<Cell>
    {

        /// <summary>
        /// Converts all <see cref="Cell"/>.<see cref="Cell.Text"/> values to List<![CDATA[<string>]]>.
        /// </summary>
        /// <returns>List<![CDATA[<string>]]> of <see cref="Cell"/>.<see cref="Cell.Text"/> values.</returns>
        public List<string> ToStringList()
        {
            List<string> res = new();

            foreach (Cell cell in this)
            {
                res.Add(cell.Text.ToString());
            }
            return res;
        }

        /// <summary>
        /// Converts all <see cref="Cell"/>.<see cref="Cell.Text"/> values to a string array.
        /// </summary>
        /// <returns>string array of <see cref="Cell"/>.<see cref="Cell.Text"/> values.</returns>
        public string[] ToStringArray()
        {
            if (this.Count == 0)
            {
                return new string[0];
            }

            string[] res = new string[this.Count];

            int idx = 0;
            foreach (Cell cell in this)
            {
                res[idx++] = cell.Text.ToString();

            }
            return res;
        }

        /// <summary>
        /// Converts all <see cref="Cell"/>.<see cref="Cell.RawText"/> values to List<![CDATA[<string>]]>.
        /// </summary>
        /// <returns>List<![CDATA[<string>]]> of <see cref="Cell"/>.<see cref="Cell.RawText"/> values.</returns>
        public List<string> RawTextToStringList()
        {
            List<string> res = new();

            foreach (Cell cell in this)
            {
                res.Add(cell.RawText.ToString());
            }
            return res;
        }

        /// <summary>
        /// Converts all <see cref="Cell"/>.<see cref="Cell.RawText"/> values to a string array.
        /// </summary>
        /// <returns>string array of <see cref="Cell"/>.<see cref="Cell.RawText"/> values.</returns>
        public string[] RawTextToStringArray()
        {
            if (this.Count == 0)
            {
                return new string[0];
            }

            string[] res = new string[this.Count];

            int idx = 0;
            foreach (Cell cell in this)
            {
                res[idx++] = cell.RawText.ToString();

            }
            return res;
        }


    }


    /// <summary>
    /// Holds results of <see cref="Explode"/>.<see cref="Explode.ExplodeString" />.
    /// </summary>
    public class ExplodeResult
    {
        /// <summary>
        /// Collection of <see cref="Cells"/> containing extracted sub strings.
        /// </summary>
        /// <value>Extracted fields.</value>
        public Cells Cells { get; }

        /// <summary>
        /// Set to true if any Cells were Text Delimited.
        /// This property is calculated by <see cref="Explode"/>.<see cref="Explode.ExplodeString"/>. Any direct modification of the
        /// <see cref="Cells"/> collection will not update this property.
        /// </summary>
        /// <value>True if any cells used a Text Delimiter.</value>
        public bool WereAnyCellsTextDelimited { get; internal set; } = false;

        /// <summary>
        /// Constructor for <c>ExplodeResult</c>.
        /// </summary>
        internal ExplodeResult()
        {
            this.Cells = new Cells();
        }

        /// <summary>
        /// Adds a cell to this collection.
        /// </summary>
        /// <param name="cell">Instance of a <c>Cell</c> to add to this collection.</param>
        internal void Add(Cell cell)
        {
            this.Cells.Add(cell);
        }
    }

    /// <summary>
    /// Holds options to used by <see cref="Explode"/>.<see cref="Explode.ExplodeString"/>.
    /// </summary>
    public class ExplodeOptions
    {
        /// <summary>
        /// Enables the processing of Text Delimiters. When enabled, <see cref="Explode"/> will process instances of the specified
        /// <see cref="Delimiters.TextDelimiter"/> to signify that any enclosed <see cref="Delimiters.FieldDelimiter"/> are taken as content.
        /// </summary>
        /// <remarks>
        /// When enabled, string <c>A,"Hello,World",B</c> will result in 3 cells, with the second cell's text value of <c>Hello,World</c>
        /// </remarks>
        /// <value>Enables the use of Text Delimiters. Default = true.</value>
        public bool EnableTextDelimiter { get; set; } = true;

        /// <summary>
        /// When enabled it is expected that the <see cref="Delimiters.TextDelimiter"/> if present, it is positioned at then beginning 
        /// and end of each field.  If disabled, any <see cref="Delimiters.TextDelimiter"/> will be processed regardless of its position.
        /// </summary>
        /// <remarks>
        /// <div class='NOTE alert alert-note'><h5>Note</h5>
        /// <p>This property is ignored if <see cref="EnableTextDelimiter"/> is disabled.</p>
        /// </div>
        /// </remarks>
        /// <value>True if the <see cref="Delimiters.TextDelimiter"/> is expected, when present, to surround an entire cell's contents.  Default value is true.</value>
        public bool StrictTextDelimiterPositioning { get; set; } = true;

        /// <summary>
        /// When enabled will allow for leading and trailing spaces before and after the Text Delimiters.
        /// </summary>
        /// <remarks>
        /// <p>Given a string of <c>A,  "one,one" ,B</c> the second cell's <see cref="Cell.Text"/> property will be set to <c>one,one</c>.</p>
        /// <div class='NOTE alert alert-note'><h5>Note</h5>
        /// <p>Only applies if <see cref="StrictTextDelimiterPositioning"/> is enabled.</p>
        /// </div>
        /// </remarks>
        /// <value>Set to true to cater for leading and trailing spaces before after <see cref="Delimiters.TextDelimiter"/>. Default value is true. </value>
        public bool StrictTextDelimiterPositioningAllowLeadingTrailingSpaces { get; set; } = true;

        /// <summary>
        /// When enabled, and when containted within text delimiters, allows for double instances of the <see cref="Delimiters.TextDelimiter"/> to 
        /// be processed as single content.
        /// </summary>
        /// <remarks>
        /// For example: when enabled string <c>A,"one,"",two",B</c> will return the second <see cref="Cell"/> with the <see cref="Cell.Text"/>  value of <c>one,",one</c>
        /// </remarks>
        /// <value>True to enable the use of Double Text Delimiters within a Text Delimited string.</value>
        public bool AllowInsideDoubleTextDelimiters { get; set; } = true;


        /// <summary>
        /// Which character or strings are being used for Field and Text Delimiters.
        /// </summary>
        /// <value>Instance of the <see cref="Delimiters"/> class.</value>
        public Delimiters Delimiters { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ExplodeOptions()
        {
            this.Delimiters = new Delimiters();
        }

        /// <param name="delimiters">Which delimiters to use.</param>
        public ExplodeOptions(Delimiters delimiters)
        {
            this.Delimiters = delimiters;
        }

    }

    /// <summary>
    /// Splits a string into substrings.
    /// </summary>
    public class Explode
    {

        /// <summary>
        /// Splits a single string into sub-strings.
        /// </summary>
        /// <param name="text">String to split.</param>
        /// <param name="opts">Options class containing the field and text delimiters as well as other options.</param>
        /// <returns>Instance of <see cref="ExplodeResult"/> containing a collection of <see cref="Cell"/> objects containing the extracted text.</returns>
        public static ExplodeResult ExplodeString(string text, ExplodeOptions opts)
        {
            return text.Explode(opts);
        }

    }

    /// <summary>
    /// The workhorse engine to split a string of text as per ExplodeOptions and Delimiters
    /// </summary>
    /// <remarks>
    /// This class is internal to hide implementation to reduce the risk of future changes breaking existing use of public classes.
    /// </remarks>

    internal class ExplodeStringEngine
    {

        private Delimiters Chrs;

        private ExplodeOptions Opts;

        private string LineText;

        private int LineLength;

        private int CurrentCharIndex = 0;

        private bool IsLastFieldProcessed = false;

        /// <summary>
        /// Initialise instance of engine ready for processing
        /// </summary>
        /// <param name="lineText"></param>
        /// <param name="opts"></param>
        public ExplodeStringEngine(string lineText, ExplodeOptions opts)
        {
            this.Opts = opts;
            this.Chrs = opts.Delimiters;

            this.LineText = lineText;
            this.LineLength = lineText.Length;
        }

        /// <summary>
        /// Returns the next Cell
        /// <see cref="IsProcessingComplete"></see>
        /// </summary>
        /// <returns>Instance of <c>Cell</c> containg the field's contents</returns>
        /// <exception cref="Exception">Will raise an exception if called when there are no more fields to be procssed</exception>
        public Cell GetNextCell()
        {
            if (this.IsAtEndOfString)
            {
                // we are at the end of the string, but IsLastFieldProcessed may not yet be set.
                // This will happen if the last character[s] in the string is a field delimiter
                // e.g. one,two,three, where 4 fields need to be returned, with the last 
                // having the value of ""
                if (!this.IsLastFieldProcessed)
                {
                    this.IsLastFieldProcessed = true;
                    return new Cell(new StringBuilder("", 1), new StringBuilder("", 1), false, false);
                }
                else
                {
                    throw new Exception("Call to GetNextCell past end of line");
                }
            }

            StringBuilder text = new(64);
            StringBuilder rawText = new(64);
            bool wasTextDelimited = false;
            bool textContainedTextDelimiter = false;

            char peekChar;

            bool withinTextDelimiter = false;
            bool processTextDelimitersForThisCell = true;

            int loopCount = 0;
            char spaceChar = ' ';
            while (!this.IsAtEndOfString)
            {
                loopCount++;

                peekChar = this.PeekChar();

                if (this.Opts.EnableTextDelimiter && peekChar == Chrs.FirstTextDelimiterChar && processTextDelimitersForThisCell)
                {
                    if (this.Chrs.IsTextDelimiterASingleChar)
                    {
                        if (!withinTextDelimiter)
                        {
                            if (1 == loopCount || this.Opts.StrictTextDelimiterPositioning == false)
                            {
                                rawText.Append(Chrs.FirstTextDelimiterChar);
                                withinTextDelimiter = true;
                                wasTextDelimited = true;
                                this.CurrentCharIndex++;
                                continue;
                            } // end: first character or not applying strict text delimiter positioning
                            else if (this.Opts.StrictTextDelimiterPositioning)
                            {
                                //Not a double text delimiter, so process end of text delimited area
                                //Strict = text delimiter is expected to be at the end of that cell's contents
                                //We do however, cater for the possability of spaces after the text delimiter before 
                                //the field delimiter (or end of line)

                                if (this.Opts.StrictTextDelimiterPositioningAllowLeadingTrailingSpaces)
                                {
                                    // if here, the we are not at the first character but have come across
                                    // a text delimiter.  Accomodating leading spaces before the first
                                    // text delimiter is enabled.  If we only have spaces so far then
                                    // allow this text delimiter to denote the start of a delimited area
                                    if ("" == text.ToString().Trim())
                                    {
                                        text.Clear();
                                        rawText.Append(Chrs.FirstTextDelimiterChar);
                                        withinTextDelimiter = true;
                                        wasTextDelimited = true;
                                        this.CurrentCharIndex++;
                                        continue;
                                    }
                                } // end: allow leading spaces 
                                else
                                {
                                    // text delimiter not at a strict position, don't process text delimiters for the remainder of this cell
                                    processTextDelimitersForThisCell = false;
                                } // end: not allowing leading spaces
                            } // end: strict text delimiter processing
                        } // end !withinTextDelimiter
                        else
                        {
                            char peekNextChar = this.PeekNextChar();

                            //if text delimited end of line, then close off delimited section
                            if ('\0' == peekNextChar)
                            {
                                rawText.Append(Chrs.FirstTextDelimiterChar);
                                this.CurrentCharIndex++;
                                withinTextDelimiter = false;
                                continue;
                            } // end: end of line

                            // We are within text delimiters, and the current characters is a text delimiter
                            // Is the next character also a text delimiter e.g. ,"one""two", where the string is: one"two
                            if (this.Opts.AllowInsideDoubleTextDelimiters && peekNextChar == Chrs.FirstTextDelimiterChar)
                            {
                                textContainedTextDelimiter = true;
                                text.Append(Chrs.FirstTextDelimiterChar);
                                rawText.Append(Chrs.FirstTextDelimiterChar);
                                rawText.Append(Chrs.FirstTextDelimiterChar);
                                this.CurrentCharIndex += 2;
                                continue;
                            } //end: is internal text delimiter
                            else if (this.Opts.StrictTextDelimiterPositioning)
                            {
                                // we are expecting the closing text delimiter to be before the field separator (or end of line)
                                int lookForFieldDelimiterAtOffset = 1;
                                if (this.Opts.StrictTextDelimiterPositioningAllowLeadingTrailingSpaces)
                                {
                                    while (spaceChar == this.PeekChar(lookForFieldDelimiterAtOffset))
                                    {
                                        lookForFieldDelimiterAtOffset++;
                                    }
                                }

                                string peekedCharsAfterTextDelimiter = this.PeekChars(lookForFieldDelimiterAtOffset, Chrs.FieldDelimiterLength);
                                if (this.Chrs.IsFieldDelimiterASingleChar)
                                {
                                    if ("" == peekedCharsAfterTextDelimiter || peekedCharsAfterTextDelimiter[0] == Chrs.FirstFieldDelimiterChar)
                                    {
                                        rawText.Append(Chrs.FirstTextDelimiterChar);
                                        if (lookForFieldDelimiterAtOffset > 1)
                                        {
                                            rawText.Append(spaceChar, lookForFieldDelimiterAtOffset - 1);
                                        }
                                        this.CurrentCharIndex += lookForFieldDelimiterAtOffset;
                                        withinTextDelimiter = false;
                                        continue;
                                    } //end: end of line or we have a two single character field delimiters next to each other
                                } // end: field separator is single char
                                else if (peekedCharsAfterTextDelimiter.Equals(this.Chrs.FieldDelimiter, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    rawText.Append(Chrs.TextDelimiter);
                                    if (lookForFieldDelimiterAtOffset > Chrs.TextDelimiterLength)
                                    {
                                        rawText.Append(spaceChar, lookForFieldDelimiterAtOffset - Chrs.TextDelimiterLength);
                                    }

                                    this.CurrentCharIndex += lookForFieldDelimiterAtOffset;
                                    withinTextDelimiter = false;
                                    continue;
                                } //end: field delimiter is multi character and we have two field delimiters next to each other
                            } //end: not internal text delimiter and StrictTextDelimiterPositioning
                            else
                            {
                                rawText.Append(Chrs.FirstTextDelimiterChar);
                                this.CurrentCharIndex++;
                                withinTextDelimiter = false;
                                continue;
                            } //end: not internal text delimiter and  !StrictTextDelimiterPositioning
                        } //end: withinTextDelimiter
                    } // end: IsTextDelimiterASingleChar
                    else
                    {
                        // text delimiter is a multiple character string... 
                        string peekString = this.PeekChars(0, Chrs.TextDelimiterLength);

                        if (peekString.Equals(Chrs.TextDelimiter, StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (!withinTextDelimiter)
                            {
                                // Are we at the beginning of this cell's contents or we do not need to 
                                // check that the text delimiter as after field delimiter
                                if ((1 == loopCount || this.Opts.StrictTextDelimiterPositioning == false))
                                {
                                    withinTextDelimiter = true;
                                    wasTextDelimited = true;
                                    this.CurrentCharIndex += Chrs.TextDelimiterLength;
                                    rawText.Append(peekString);
                                    continue;
                                } // end: first character in cell or not using strict text delimter positioning
                                else if (this.Opts.StrictTextDelimiterPositioning)
                                {
                                    if (this.Opts.StrictTextDelimiterPositioningAllowLeadingTrailingSpaces)
                                    {
                                        // if here, the we are not at the first character but have come across
                                        // a text delimiter.  Accomodating leading spaces before the first
                                        // text delimiter is enabled.  If we only have spaces so far then
                                        // allow this text delimiter to denote the start of a delimited area
                                        if ("" == text.ToString().Trim())
                                        {
                                            text.Clear();
                                            withinTextDelimiter = true;
                                            wasTextDelimited = true;
                                            this.CurrentCharIndex += Chrs.TextDelimiterLength;
                                            rawText.Append(peekString);
                                            continue;
                                        } //end: removal of leading spaces required
                                    } // end: allow leading spaces 
                                    else
                                    {
                                        // text delimiter not at a strict position, don't process text delimiters for the remainder of this cell
                                        processTextDelimitersForThisCell = false;
                                    } // end: not allowing leading spaces
                                } // end: StrictTextDelimiterPositioning
                            } // end: not within text delimited area
                            else
                            {
                                string peekDoubleString = this.PeekChars(0, Chrs.TextDelimiterLength + Chrs.TextDelimiterLength);

                                // not enought characters left in line, therefore cannot be a double text delimiter
                                if (peekDoubleString == "")
                                {
                                    //if text delimited end of line, then close off delimited section
                                    rawText.Append(Chrs.TextDelimiter);
                                    this.CurrentCharIndex += Chrs.TextDelimiterLength;
                                    withinTextDelimiter = false;
                                    continue;
                                } // end: check for end of/too close to end of line

                                // We are within text delimiters, and the current characters is a text delimiter
                                // Are the next character also a text delimiter e.g. ,<>one<><>two<>, where the string is: one<>'two
                                if (this.Opts.AllowInsideDoubleTextDelimiters && peekDoubleString.Equals(Chrs.TextDelimiter + Chrs.TextDelimiter, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    text.Append(Chrs.TextDelimiter);
                                    rawText.Append(peekDoubleString);
                                    textContainedTextDelimiter = true;
                                    this.CurrentCharIndex += peekDoubleString.Length;
                                    continue;
                                } // end: is double text delimiter
                                else
                                {
                                    //Not a double text delimiter, so process end of text delimited area
                                    //Strict = text delimiter is expected to be at the end of that cell's contents
                                    //We do however, cater for the possability of spaces after the text delimiter before 
                                    //the field delimiter (or end of line)
                                    if (this.Opts.StrictTextDelimiterPositioning)
                                    {
                                        int lookForFieldDelimiterAtOffset = Chrs.TextDelimiterLength;
                                        if (this.Opts.StrictTextDelimiterPositioningAllowLeadingTrailingSpaces)
                                        {
                                            while (spaceChar == this.PeekChar(lookForFieldDelimiterAtOffset))
                                            {
                                                lookForFieldDelimiterAtOffset++;
                                            }
                                        } //end: do check for trailing spaces?

                                        string peekedCharsAfterTextDelimiter = this.PeekChars(lookForFieldDelimiterAtOffset, Chrs.FieldDelimiterLength);
                                        if (this.Chrs.IsFieldDelimiterASingleChar)
                                        {
                                            // peekedCharsAfterTextDelimiter will be "" if end of string has been reached
                                            if ("" == peekedCharsAfterTextDelimiter || peekedCharsAfterTextDelimiter[0] == Chrs.FirstFieldDelimiterChar)
                                            {
                                                rawText.Append(Chrs.TextDelimiter);
                                                if (lookForFieldDelimiterAtOffset > Chrs.TextDelimiterLength)
                                                {
                                                    rawText.Append(spaceChar, lookForFieldDelimiterAtOffset - Chrs.TextDelimiterLength);
                                                }

                                                this.CurrentCharIndex += lookForFieldDelimiterAtOffset;
                                                withinTextDelimiter = false;
                                                continue;
                                            } //end: is end of line or matches field delimiter
                                        } // end: field delimiter is a single character
                                        else if (peekedCharsAfterTextDelimiter.Equals(this.Chrs.FieldDelimiter, StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            rawText.Append(Chrs.TextDelimiter);
                                            if (lookForFieldDelimiterAtOffset > Chrs.TextDelimiterLength)
                                            {
                                                rawText.Append(spaceChar, lookForFieldDelimiterAtOffset - Chrs.TextDelimiterLength);
                                            }

                                            this.CurrentCharIndex += lookForFieldDelimiterAtOffset;
                                            withinTextDelimiter = false;
                                            continue;
                                        } // end: matches with multi character field delimiter
                                    } // end: StrictTextDelimiterPositioning
                                    else
                                    {
                                        // Either at end of string, or not strict text delimiter positioning
                                        rawText.Append(Chrs.TextDelimiter);
                                        this.CurrentCharIndex += Chrs.TextDelimiterLength;
                                        withinTextDelimiter = false;
                                        continue;

                                    } // end: not StrictTextDelimiterPositioning
                                } // end: not a double text delimiter
                            } // end: are within text delimited area
                        } // end: matches text delimiter
                    } //end: text delimiter is multi character
                } // end: is enabled and is this a text delimiter

                // is this possibly a field delimiter (checks first character for single and multi character field delimiter)
                if (peekChar == Chrs.FirstFieldDelimiterChar)
                {
                    if (!withinTextDelimiter)
                    {
                        if (Chrs.IsFieldDelimiterASingleChar)
                        {
                            this.CurrentCharIndex++;
                            return new Cell(text, rawText, wasTextDelimited, textContainedTextDelimiter);
                        } //end: field delimiter is single char
                        else
                        {
                            string peekStr = this.PeekChars(0, Chrs.FieldDelimiterLength);

                            if (Chrs.FieldDelimiter.Equals(peekStr, StringComparison.InvariantCultureIgnoreCase))
                            {
                                this.CurrentCharIndex += Chrs.FieldDelimiterLength;
                                return new Cell(text, rawText, wasTextDelimited, textContainedTextDelimiter);
                            }
                        } //end: field delimiter is multi char
                    } //end: within text delimited area
                } //end: matches field delimiter's first character


                text.Append(peekChar);
                rawText.Append(peekChar);
                this.CurrentCharIndex++;
            } //end: while !IsAtEndOfString

            this.IsLastFieldProcessed = true;
            return new Cell(text, rawText, wasTextDelimited, textContainedTextDelimiter);
        }

        /// <summary>
        /// Return the current character.
        /// </summary>
        /// <returns>Character at the current character index within the string being processed.</returns>
        private char PeekChar()
        {
            return this.LineText[this.CurrentCharIndex];
        }

        /// <summary>
        /// Returns character at offset position
        /// </summary>
        /// <param name="offset">How many characters from the current character index to start at</param>
        /// <returns>Character at the current character index + offset within the string being procssed.  
        /// If the index + offset is out of range <c>\0</c> is returned.</returns>
        private char PeekChar(int offset)
        {
            int idx = this.CurrentCharIndex + offset;
            if (idx >= 0 && idx < this.LineLength)
            {
                return this.LineText[this.CurrentCharIndex + offset];
            }
            else
            {
                return '\0';
            }
        }

        /// <summary>
        /// Returns exactly 'x' characters at offset position or ""
        /// </summary>
        /// <param name="offset">How many characters from the current character index to start at</param>
        /// <param name="length">The number of characters to return</param>
        /// <returns>The number of characters specific at the current character index + offset.
        /// If the index + offset is out or range, or there are no enough characters in the string at that position then "" is returned.
        /// </returns>
        private string PeekChars(int offset, int length)
        {
            int idx = this.CurrentCharIndex + offset;

            if (idx >= 0 && (idx + length <= this.LineLength))
            {
                return this.LineText.Substring(idx, length);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Returns the next character.
        /// </summary>
        /// <returns>The character after the current character index within the string being processed.  
        /// If no such character exists then <c>\0</c> is returned.</returns>
        private char PeekNextChar()
        {
            if ((this.CurrentCharIndex + 1) < this.LineLength)
            {

                return this.LineText[this.CurrentCharIndex + 1];
            }
            else
            {
                return '\0';
            }
        }


        /// <summary>
        /// Checks of the internal character index point is at or past the end of the string
        /// </summary>
        /// <value>When property <c>IsAtEndOfString</c> is true we have reached the end of the string</value>
        private bool IsAtEndOfString
        {
            get
            {
                return (this.CurrentCharIndex >= this.LineLength);
            }
        }

        /// <summary>
        /// Returns true if the string pointer is at the end of the string and we have fully completing its processing
        /// </summary>
        /// <value>When property <c>IsProcessingComplete</c> is true we have completed the splitting of the string</value>
        internal bool IsProcessingComplete
        {
            get
            {
                if (this.IsAtEndOfString && this.IsLastFieldProcessed)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }


    /// <summary>
    /// Static class to add extended methods to System.String.
    /// </summary>
    public static class String
    {
        /// <summary>
        /// Splits a single string into sub-strings.
        /// </summary>
        /// <param name="text">String to split.</param>
        /// <param name="opts">Options class containing the field and text delimiters as well as other options.</param>
        /// <returns>Instance of <see cref="ExplodeResult"/> containing a collection of <see cref="Cell"/> objects containing the extracted text.</returns>
        /// <example>
        /// <code>
        ///     string s = "one,\"two.1,two.2\",three";
        ///     var sr = s.Explode(new ExplodeOptions());
        ///     var <![CDATA[List<string>]]> = sr.Cells.ToStringList();
        ///     Assert.Equal(3, sl.Count);
        ///     Assert.Equal("one", sl[0]);
        ///     Assert.Equal("two.1,two.2", sl[1]);
        ///     Assert.Equal("three", sl[2]);
        /// </code>
        /// </example>
        public static ExplodeResult Explode(this string text, ExplodeOptions opts)
        {
            ExplodeResult res = new();
            ExplodeStringEngine engine = new(text, opts);

            if (opts.EnableTextDelimiter && 0 == opts.Delimiters.TextDelimiterLength)
            {
                throw new Exception("Call to Split when Text Delimiter character has not been set");
            }

            while (!engine.IsProcessingComplete)
            {
                Cell cell = engine.GetNextCell();
                res.Add(cell);

                //Set flag is any cells contained within text delimiter[s]
                if (cell.WasTextDelimited && !res.WereAnyCellsTextDelimited)
                {
                    res.WereAnyCellsTextDelimited = true;
                }
            }

            return res;

        }
    }

}