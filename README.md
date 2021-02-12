# Sprocket.Text.Explode.dll v1.0.0 API documentation

Created by [David Pullin](https://ict-man.me)

Readme Last Updated on 12.02.2021

The full API can be found at https://ict-man.me/sprocket/api/Sprocket.Text.Explode.html

Licence [GPL-3.0 License](https://www.gnu.org/licenses/gpl-3.0.en.html)

Source code is hosted on GitHub here: https://github.com/david-pullin/ascii-calendar


<br>

# Summary

Explode provides an extension function to the String class to split strings via a specified character or string.  Useful for processing Comma Delimited (CSV) content.

Support is provided for text delimiters, for example, file that make use of the " character enclose strings that contain the field separator.

<br>


# Examples

Namespace: Sprocket.Text.Explode

### Example with default field delimiters

    string s = "one,\"two.1,two.2\",three";

    var sr = s.Explode(new ExplodeOptions());
    List<string> sl = sr.Cells.ToStringList();

    Assert.Equal(3, sl.Count);
    Assert.Equal("one", sl[0]);
    Assert.Equal("two.1,two.2", sl[1]);
    Assert.Equal("three", sl[2]);

### Example with a multiple character field delimiter

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

<br>
<br>

---
Note: The full API can be found at https://ict-man.me/sprocket/api/Sprocket.Text.Explode.html

---

<br>
<br>

# ExplodeOptions Class

Namespace: Sprocket.Text.Explode

## Properties

| Name | Type | Default | Summary |
|---|---|---|---|
| **EnableTextDelimiter** | bool | true | Enables the processing of Text Delimiters. When enabled, **Explode** will process instances of the specified **Delimiters.TextDelimiter** to signify that any enclosed **Delimiters.FieldDelimiter** are taken as content. |
| **StrictTextDelimiterPositioning** | bool | true | When enabled it is expected that the **Delimiters.TextDelimiter** (if present), it is positioned at then beginning and end of each field.  If disabled, any **Delimiters.TextDelimiter** will be processed as a delimiter regardless of its position. |
| **StrictTextDelimiterPositioning AllowLeadingTrailingSpaces** | bool | true | When enabled will allow for leading and trailing spaces before and after the Text Delimiters. |
| **AllowInsideDoubleTextDelimiters** | bool | true | When enabled, and when containted within text delimiters, allows for double instances of the **Delimiters.TextDelimiter** to <br>be processed as single instance content. |
| **Delimiters** | [Delimiters](Sprocket.Text.Explode.Delimiters.html) | , and " | Which character or strings are being used for Field and Text Delimiters. |

