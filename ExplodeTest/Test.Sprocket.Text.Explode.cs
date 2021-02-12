using System.Collections.Generic;
using Xunit;

using Sprocket.Text;
using Sprocket.Text.Explode;

namespace skCSVTest
{
    public class TestSplitLine
    {
        #region "Character Based Separators and TextDelimiters"
        [Fact]
        public void CharacterSimpleTest()
        {
            var opts = new ExplodeOptions(new Delimiters());
            var text = "one,two,three";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.False(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two", sl[1]);
            Assert.Equal("three", sl[2]);
        }

        [Fact]
        public void CharacterEmptyFieldAtEnd()
        {
            var opts = new ExplodeOptions(new Delimiters());
            var text = "one,two,three,";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.False(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(4, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two", sl[1]);
            Assert.Equal("three", sl[2]);
            Assert.Equal("", sl[3]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(4, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two", sl[1]);
            Assert.Equal("three", sl[2]);
            Assert.Equal("", sl[3]);
        }

        [Fact]
        public void CharacterJustSeparators()
        {
            var opts = new ExplodeOptions(new Delimiters());
            var text = ",,,";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.False(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(4, sl.Count);
            Assert.Equal("", sl[0]);
            Assert.Equal("", sl[1]);
            Assert.Equal("", sl[2]);
            Assert.Equal("", sl[3]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(4, sl.Count);
            Assert.Equal("", sl[0]);
            Assert.Equal("", sl[1]);
            Assert.Equal("", sl[2]);
            Assert.Equal("", sl[3]);
        }

        [Fact]
        public void CharacterDifferentSeperator()
        {
            var opts = new ExplodeOptions(new Delimiters(':'));
            var text = "one:two:three";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.False(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two", sl[1]);
            Assert.Equal("three", sl[2]);
        }

        [Fact]
        public void CharacterTextDelimiters()
        {
            var opts = new ExplodeOptions(new Delimiters());
            var text = "one,\"two\",three";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("\"two\"", sl[1]);
            Assert.Equal("three", sl[2]);
        }

        [Fact]
        public void CharacterTextDelimitersWithEmbeddedSeparator()
        {
            var opts = new ExplodeOptions(new Delimiters());
            var text = "one,\"two,comma\",three";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two,comma", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("\"two,comma\"", sl[1]);
            Assert.Equal("three", sl[2]);
        }

        [Fact]
        public void CharacterTextDelimitersWithEmbeddedTextDelimiter()
        {
            var opts = new ExplodeOptions(new Delimiters());
            var text = @"one,""two""""quote"",three";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal(@"two""quote", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("\"two\"\"quote\"", sl[1]);
            Assert.Equal("three", sl[2]);
        }

        [Fact]
        public void CharacterTextDelimitersWithEmbeddedTextDelimiterAndFieldSeparator()
        {
            var opts = new ExplodeOptions(new Delimiters());
            var text = @"one,""two"""",q,uote"",three";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal(@"two"",q,uote", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal(@"""two"""",q,uote""", sl[1]);
            Assert.Equal("three", sl[2]);
        }
        #endregion

        #region "String Based Separators and TextDelimiters"

        [Fact]
        public void StringSimpleTest()
        {
            var opts = new ExplodeOptions(new Delimiters("_x_"));
            var text = "one_x_two_x_three";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.False(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two", sl[1]);
            Assert.Equal("three", sl[2]);
        }

        [Fact]
        public void StringEmptyFieldAtEnd()
        {
            var opts = new ExplodeOptions(new Delimiters("_x_"));
            var text = "one_x_two_x_three_x_";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.False(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(4, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two", sl[1]);
            Assert.Equal("three", sl[2]);
            Assert.Equal("", sl[3]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(4, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two", sl[1]);
            Assert.Equal("three", sl[2]);
            Assert.Equal("", sl[3]);
        }

        [Fact]
        public void StringJustSeparators()
        {
            var opts = new ExplodeOptions(new Delimiters("_x_"));
            var text = "_x__x__x_";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.False(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(4, sl.Count);
            Assert.Equal("", sl[0]);
            Assert.Equal("", sl[1]);
            Assert.Equal("", sl[2]);
            Assert.Equal("", sl[3]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(4, sl.Count);
            Assert.Equal("", sl[0]);
            Assert.Equal("", sl[1]);
            Assert.Equal("", sl[2]);
            Assert.Equal("", sl[3]);
        }

        [Fact]
        public void StringTextDelimiters()
        {

            var opts = new ExplodeOptions(new Delimiters("_x_", "<>"));
            var text = "one_x_<>two<>_x_three";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("<>two<>", sl[1]);
            Assert.Equal("three", sl[2]);
        }

        [Fact]
        public void StringTextDelimitersWithEmbeddedSeparator()
        {
            var opts = new ExplodeOptions(new Delimiters(",", "_x_"));
            var text = "one,_x_two,comma_x_,three";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two,comma", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("_x_two,comma_x_", sl[1]);
            Assert.Equal("three", sl[2]);
        }

        [Fact]
        public void StringTextDelimitersWithEmbeddedTextDelimiter()
        {
            var opts = new ExplodeOptions(new Delimiters(",", "_x_"));
            var text = @"one,_x_two_x__x_quote_x_,three";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two_x_quote", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("_x_two_x__x_quote_x_", sl[1]);
            Assert.Equal("three", sl[2]);
        }

        [Fact]
        public void StringTextDelimitersWithEmbeddedTextDelimiterAndFieldSeparator()
        {
            var opts = new ExplodeOptions(new Delimiters(",", "_x_"));
            var text = @"one,_x_two_x__x_,q,uote_x_,three";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two_x_,q,uote", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("_x_two_x__x_,q,uote_x_", sl[1]);
            Assert.Equal("three", sl[2]);
        }
        #endregion

        #region "Option EnableTextDelimiter"

        [Fact]
        public void TextDelimiterDisabled()
        {
            var opts = new ExplodeOptions(new Delimiters());
            opts.EnableTextDelimiter = false;

            var text = @"one,""two"",three";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.False(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal(@"""two""", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal(@"""two""", sl[1]);
            Assert.Equal("three", sl[2]);

        }

        [Fact]
        public void TextDelimiterDisabledWithInsideSeparator()
        {
            var opts = new ExplodeOptions(new Delimiters());
            opts.EnableTextDelimiter = false;

            var text = @"one,""t,wo"",three";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.False(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(4, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal(@"""t", sl[1]);
            Assert.Equal(@"wo""", sl[2]);
            Assert.Equal("three", sl[3]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(4, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal(@"""t", sl[1]);
            Assert.Equal(@"wo""", sl[2]);
            Assert.Equal("three", sl[3]);

        }

        #endregion

        #region "Option AllowInsideDoubleTextDelimiters"

        [Fact]
        public void InsideDoubleTextDelimitersDisabledStrictPositioningEnabled()
        {
            var opts = new ExplodeOptions(new Delimiters());
            opts.AllowInsideDoubleTextDelimiters = false;
            opts.StrictTextDelimiterPositioning = true;

            var text = @"one,""t""""wo"",three";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal(@"t""""wo", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("\"t\"\"wo\"", sl[1]);
            Assert.Equal("three", sl[2]);

        }

        [Fact]
        public void InsideDoubleTextDelimitersDisabledStrictPositioningEnabledAsLastField()
        {
            var opts = new ExplodeOptions(new Delimiters());
            opts.AllowInsideDoubleTextDelimiters = false;
            opts.StrictTextDelimiterPositioning = true;

            var text = @"one,""t""""wo""";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(2, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal(@"t""""wo", sl[1]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(2, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("\"t\"\"wo\"", sl[1]);
        }


        [Fact]
        public void InsideDoubleTextDelimitersDisabledStrictPositioningDisabled()
        {
            var opts = new ExplodeOptions(new Delimiters());
            opts.AllowInsideDoubleTextDelimiters = false;
            opts.StrictTextDelimiterPositioning = false;

            var text = @"one,""t""""wo"",three";

            //  "       in string
            //  t       take t
            //  "       out string
            //  "       in string
            //  w       take w
            //  o       take o
            //  "       out string

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal(@"two", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("\"t\"\"wo\"", sl[1]);
            Assert.Equal("three", sl[2]);
        }

        [Fact]
        public void InsideDoubleTextDelimitersDisabledStrictPositioningDisabledAsLastField()
        {
            var opts = new ExplodeOptions(new Delimiters());
            opts.AllowInsideDoubleTextDelimiters = false;
            opts.StrictTextDelimiterPositioning = false;

            var text = @"one,""t""""wo""";

            //  "       in string
            //  t       take t
            //  "       out string
            //  "       in string
            //  w       take w
            //  o       take o
            //  "       out string

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(2, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal(@"two", sl[1]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(2, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("\"t\"\"wo\"", sl[1]);
        }


        [Fact]
        public void InsideDoubleTextDelimitersEnablesStrictPositioningEnabled()
        {
            var opts = new ExplodeOptions(new Delimiters());
            opts.AllowInsideDoubleTextDelimiters = true;
            opts.StrictTextDelimiterPositioning = true;

            var text = @"one,""t""""wo"",three";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("t\"wo", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("\"t\"\"wo\"", sl[1]);
            Assert.Equal("three", sl[2]);

        }

        [Fact]
        public void InsideDoubleTextDelimitersEnablesStrictPositioningEnabledAsLastField()
        {
            var opts = new ExplodeOptions(new Delimiters());
            opts.AllowInsideDoubleTextDelimiters = true;
            opts.StrictTextDelimiterPositioning = true;

            var text = @"one,""t""""wo""";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(2, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("t\"wo", sl[1]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(2, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("\"t\"\"wo\"", sl[1]);

        }

        [Fact]
        public void InsideDoubleTextDelimitersEnablesStrictPositioningDisabled()
        {
            var opts = new ExplodeOptions(new Delimiters());
            opts.AllowInsideDoubleTextDelimiters = true;
            opts.StrictTextDelimiterPositioning = true;

            var text = @"one,""t""""wo"",three";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("t\"wo", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("\"t\"\"wo\"", sl[1]);
            Assert.Equal("three", sl[2]);

        }

        [Fact]
        public void InsideDoubleTextDelimitersEnablesStrictPositioningDisabledAsLastField()
        {
            var opts = new ExplodeOptions(new Delimiters());
            opts.AllowInsideDoubleTextDelimiters = true;
            opts.StrictTextDelimiterPositioning = true;

            var text = @"one,""t""""wo""";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(2, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("t\"wo", sl[1]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(2, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("\"t\"\"wo\"", sl[1]);
        }



        #endregion

        #region "StrictTextDelimiterPositioning"

        [Fact]
        public void StrictTextDelimiterDisabledStandardQuotedCell()
        {
            var opts = new ExplodeOptions(new Delimiters());
            opts.StrictTextDelimiterPositioning = false;

            var text = @"one,""two"",three";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("\"two\"", sl[1]);
            Assert.Equal("three", sl[2]);

        }

        [Fact]
        public void StrictTextDelimiterDisabledStandardQuotedCellMultiCharTextDelimiter()
        {
            var opts = new ExplodeOptions(new Delimiters(',', "xx"));
            opts.StrictTextDelimiterPositioning = false;

            var text = @"one,xxtwoxx,three";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("xxtwoxx", sl[1]);
            Assert.Equal("three", sl[2]);

        }



        [Fact]
        public void StrictTextDelimiterEnabledQuotesWithinCell()
        {
            var opts = new ExplodeOptions(new Delimiters());
            opts.StrictTextDelimiterPositioning = true;

            var text = @"one,t""wo"",three";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.False(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("t\"wo\"", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("t\"wo\"", sl[1]);
            Assert.Equal("three", sl[2]);

        }

        [Fact]
        public void StrictTextDelimiterEnabledQuotesWithinCellMultiCharTextDelimiter()
        {
            var opts = new ExplodeOptions(new Delimiters(',', "xx"));
            opts.StrictTextDelimiterPositioning = true;

            var text = @"one,txxwoxx,three";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.False(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("txxwoxx", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("txxwoxx", sl[1]);
            Assert.Equal("three", sl[2]);

        }


        [Fact]
        public void StringTextDelimiterDisabledQuotesWithinCell()
        {
            var opts = new ExplodeOptions(new Delimiters());
            opts.StrictTextDelimiterPositioning = false;

            var text = @"one,t""wo"",three";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("t\"wo\"", sl[1]);
            Assert.Equal("three", sl[2]);

        }


        [Fact]
        public void StringTextDelimiterDisabledQuotesWithinCellMultiCharTextDelimiter()
        {
            var opts = new ExplodeOptions(new Delimiters(',', "xx"));
            opts.StrictTextDelimiterPositioning = false;

            var text = @"one,txxwoxx,three";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("txxwoxx", sl[1]);
            Assert.Equal("three", sl[2]);
        }


        [Fact]
        public void StrictTextDelimiterPositioningEnabledSpacesEnabledWithLeadingSpaces()
        {
            var opts = new ExplodeOptions(new Delimiters());
            opts.StrictTextDelimiterPositioning = true;
            opts.StrictTextDelimiterPositioningAllowLeadingTrailingSpaces = true;

            var text = @"one,    ""two"",three";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("    \"two\"", sl[1]);
            Assert.Equal("three", sl[2]);
        }

        [Fact]
        public void StrictTextDelimiterPositioningEnabledSpacesEnabledWithLeadingSpacesMultiCharTextDelimiter()
        {
            var opts = new ExplodeOptions(new Delimiters(',', "xx"));
            opts.StrictTextDelimiterPositioning = true;
            opts.StrictTextDelimiterPositioningAllowLeadingTrailingSpaces = true;

            var text = @"one,    xxtwoxx,three";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("    xxtwoxx", sl[1]);
            Assert.Equal("three", sl[2]);
        }

        [Fact]
        public void StrictTextDelimiterPositioningEnabledSpacesEnabledWithTrailingSpaces()
        {
            var opts = new ExplodeOptions(new Delimiters());
            opts.StrictTextDelimiterPositioning = true;
            opts.StrictTextDelimiterPositioningAllowLeadingTrailingSpaces = true;

            var text = @"one,    ""two""  ,three";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("    \"two\"  ", sl[1]);
            Assert.Equal("three", sl[2]);
        }

        [Fact]
        public void StrictTextDelimiterPositioningEnabledSpacesEnabledWithTrailingSpacesMultiCharTextDelimiter()
        {
            var opts = new ExplodeOptions(new Delimiters(',', "xx"));
            opts.StrictTextDelimiterPositioning = true;
            opts.StrictTextDelimiterPositioningAllowLeadingTrailingSpaces = true;

            var text = @"one,    xxtwoxx  ,three";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("    xxtwoxx  ", sl[1]);
            Assert.Equal("three", sl[2]);
        }

        [Fact]
        public void StrictTextDelimiterPositioningEnabledSpacesEnabledWithTrailingSpacesMultiCharTextDelimiterMultiCharSeperator()
        {
            var opts = new ExplodeOptions(new Delimiters("_x_", "xx"));
            opts.StrictTextDelimiterPositioning = true;
            opts.StrictTextDelimiterPositioningAllowLeadingTrailingSpaces = true;

            var text = @"one_x_    xxtwoxx  _x_three";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("    xxtwoxx  ", sl[1]);
            Assert.Equal("three", sl[2]);
        }

        [Fact]
        public void StrictTextDelimiterPositioningEnabledSpacesEnabledWithTrailingSpacesMultiCharSeperator()
        {
            var opts = new ExplodeOptions(new Delimiters("_x_"));
            opts.StrictTextDelimiterPositioning = true;
            opts.StrictTextDelimiterPositioningAllowLeadingTrailingSpaces = true;

            var text = @"one_x_    ""two""  _x_three";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("    \"two\"  ", sl[1]);
            Assert.Equal("three", sl[2]);
        }
        #endregion

        #region "String Termination Tests"


        [Fact]
        private void TerminatedWithTextDelimitedField()
        {

            var opts = new ExplodeOptions(new Delimiters());

            //defaults
            opts.StrictTextDelimiterPositioning = true;
            opts.StrictTextDelimiterPositioningAllowLeadingTrailingSpaces = true;

            var text = @"one,""two"",""three""";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("\"two\"", sl[1]);
            Assert.Equal("\"three\"", sl[2]);
        }

        [Fact]
        private void TerminatedWithTextDelimitedFieldTrailingSpaces()
        {

            var opts = new ExplodeOptions(new Delimiters());

            //defaults
            opts.StrictTextDelimiterPositioning = true;
            opts.StrictTextDelimiterPositioningAllowLeadingTrailingSpaces = true;

            var text = @"one,""two"",""three""  ";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("\"two\"", sl[1]);
            Assert.Equal("\"three\"  ", sl[2]);
        }

        [Fact]
        private void TerminatedWithTextDelimitedFieldMultiCharTextDelimiter()
        {

            var opts = new ExplodeOptions(new Delimiters(',', "<>"));

            //defaults
            opts.StrictTextDelimiterPositioning = true;
            opts.StrictTextDelimiterPositioningAllowLeadingTrailingSpaces = true;

            var text = @"one,<>two<>,<>three<>";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("<>two<>", sl[1]);
            Assert.Equal("<>three<>", sl[2]);
        }

        [Fact]
        private void TerminatedWithTextDelimitedFieldMultiCharTextDelimiterTrailingSpaces()
        {

            var opts = new ExplodeOptions(new Delimiters(',', "<>"));

            //defaults
            opts.StrictTextDelimiterPositioning = true;
            opts.StrictTextDelimiterPositioningAllowLeadingTrailingSpaces = true;

            var text = @"one,<>two<>,<>three<>   ";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two", sl[1]);
            Assert.Equal("three", sl[2]);

            sl = sr.Cells.RawTextToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("<>two<>", sl[1]);
            Assert.Equal("<>three<>   ", sl[2]);
        }
        #endregion

        #region "Extended String Method"
        [Fact]
        public void ExtendedStringMethodExample1()
        {
            string s = "one,\"two.1,two.2\",three";

            var sr = s.Explode(new ExplodeOptions());
            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two.1,two.2", sl[1]);
            Assert.Equal("three", sl[2]);
        }

        [Fact]
        public void ExtendedStringMethodExample2()
        {
            string s = "one_x_two_x_three";

            var delim = new Delimiters("_x_");
            var opts = new ExplodeOptions();
            opts.Delimiters = delim;
            var sr = s.Explode(opts);
            List<string> sl = sr.Cells.ToStringList();

            Assert.Equal(3, sl.Count);
            Assert.Equal("one", sl[0]);
            Assert.Equal("two", sl[1]);
            Assert.Equal("three", sl[2]);
        }
        #endregion

        #region "ToArray Tests"

        [Fact]
        private void TextToStringArrayTest()
        {
            var opts = new ExplodeOptions(new Delimiters());

            //defaults
            opts.StrictTextDelimiterPositioning = true;
            opts.StrictTextDelimiterPositioningAllowLeadingTrailingSpaces = true;

            var text = @"one,""two"",""three""";

            ExplodeResult sr = Explode.ExplodeString(text, opts);
            Assert.True(sr.WereAnyCellsTextDelimited);

            string[] sa = sr.Cells.ToStringArray();

            Assert.Equal(3, sa.Length);
            Assert.Equal("one", sa[0]);
            Assert.Equal("two", sa[1]);
            Assert.Equal("three", sa[2]);

            sa = sr.Cells.RawTextToStringArray();

            Assert.Equal(3, sa.Length);
            Assert.Equal("one", sa[0]);
            Assert.Equal("\"two\"", sa[1]);
            Assert.Equal("\"three\"", sa[2]);

        }
        #endregion

    }
}
