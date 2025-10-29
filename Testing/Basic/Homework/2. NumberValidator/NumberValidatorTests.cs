using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 2, false, TestName = "PrecisionIsNegative")]  
    [TestCase(1, 2, false, TestName = "ScaleGreaterThanPrecision")]
    [TestCase(1, 1, false, TestName = "ScaleEqualsPrecision")]  
    [TestCase(1, -1, false, TestName = "NegativeScale")] 
    [TestCase(0, 2, false, TestName = "ZeroPrecision")]
    public void Constructor_WithInvalidArguments_ThrowsArgumentException(int precision, int scale, bool onlyPositive)
    {   
        var createNumberValidator = () => new NumberValidator(precision, scale, onlyPositive);

        createNumberValidator.Should().Throw<ArgumentException>();
    }
    
    [TestCase(30, 2, true, "0.0", TestName = "DecimalValue")]
    [TestCase(30, 2, true, "0,0", TestName = "DecimalValueWithComma")]
    [TestCase(30, 2, true, "0", TestName = "OneDigitValue")]
    [TestCase(30, 2, true,"00.00", TestName = "MultipleDigitsInWholePart")]
    [TestCase(30, 2, true, "922337203685477580711.0", TestName = "VeryLargeWholePartNumber")]
    [TestCase(31, 30, true, "0.922337203685477580711", TestName = "VeryLargeFractionalPartNumber")]
    [TestCase(30, 2, false, "-0.0", TestName = "NegativeValueWithOnlyPositiveFalse")]
    public void IsValidNumber_WithValidArguments_ReturnsTrue(int precision, int scale, bool onlyPositive, string value)
    {
        var numberValidator = new NumberValidator(precision, scale, onlyPositive);
        
        var isValidNumber = numberValidator.IsValidNumber(value);

        isValidNumber.Should().BeTrue();
    }
    
    [TestCase(3, 2, true, "00.00", TestName = "PrecisionLessThanValueLength")]
    [TestCase(30, 2, true, "0.922", TestName = "FractionalPartNumberGreaterScale")]
    [TestCase(30, 2, true, "a.sd", TestName = "Letters")]
    [TestCase(30, 2, true, "1.sd", TestName = "MixedNumericAndLetters")]
    [TestCase(30, 2, true, "1.\n", TestName = "MixedNumericAndControlCharacter")]
    [TestCase(4, 2, true, "+\n.10", TestName = "MixedNumericAndPositiveLeadingControlCharacter")]
    [TestCase(30, 2, true, "🛠.🛠🛠", TestName = "HTMLSymbols")]
    [TestCase(30, 2, true, "", TestName = "EmptyValue")]
    [TestCase(30, 2, true, null, TestName = "NullValue")]
    [TestCase(30, 2, true, "Hello world!", TestName = "Text")]
    [TestCase(30, 2, true, "0+0", TestName = "InvalidSignPosition")]
    [TestCase(4, 2, true, "+-0.0", TestName = "DuplicateSigns")]
    [TestCase(30, 2, true, "+++", TestName = "MultipleSigns")]
    [TestCase(30, 2, true, "   ", TestName = "WhitespaceOnly")]
    [TestCase(30, 2, true, ".12", TestName = "MissingIntegerPart")]
    [TestCase(4, 2, true, "-1.23", TestName = "NegativeValueWithOnlyPositive")]
    [TestCase(6, 3, true, "+1 .2 3", TestName = "NumberWithEmptySpace")]
    public void IsValidNumber_WithInvalidArguments_ReturnsFalse(int precision, int scale, bool onlyPositive, string value)
    {   
        var numberValidator = new NumberValidator(precision, scale, onlyPositive);

        var isValidNumber = numberValidator.IsValidNumber(value);

        isValidNumber.Should().BeFalse();
    }
}