using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(1, 0, true, TestName = "OnlyPositiveIsTrue")]
    [TestCase(1, 0, false, TestName = "OnlyPositiveIsFalse")] 
    [TestCase(int.MaxValue, 0, false, TestName = "PrecisionMaxValue")] 
    public void Constructor_WhenValidArguments_ReturnVoid(int precision, int scale, bool onlyPositive)
    {   
        var func = () => new NumberValidator(precision, scale, onlyPositive);

        func.Should().NotThrow();
    }
    
    [TestCase(-1, 2, false, TestName = "PrecisionIsNegative")]  
    [TestCase(1, 2, false, TestName = "ScaleGreaterThanPrecision")]
    [TestCase(1, 1, false, TestName = "ScaleEqualsPrecision")]  
    [TestCase(1, -1, false, TestName = "NegativeScale")] 
    public void Constructor_WithInvalidArguments_ThrowsArgumentException(int precision, int scale, bool onlyPositive)
    {   
        var func = () => new NumberValidator(precision, scale, onlyPositive);

        func.Should().Throw<ArgumentException>();
    }
    
    [TestCase(30, 2, true, "0.0", ExpectedResult = true, TestName = "DecimalValue")]
    [TestCase(30, 2, true, "0", ExpectedResult = true, TestName = "OneDigitValue")]
    [TestCase(30, 2, true,"00.00", ExpectedResult = true, TestName = "MultipleDigitsInWholePart")]
    [TestCase(3, 2, true, "+1.23", ExpectedResult = false, TestName = "PositiveValue")]
    [TestCase(4, 2, true, "-1.23", ExpectedResult = false, TestName = "NegativeValue")]
    [TestCase(6, 3, true, "-1 .2 3", ExpectedResult = false, TestName = "NumberWithEmptySpace")]
    [TestCase(30, 2, true, "922337203685477580711.0", ExpectedResult = true, TestName = "VeryLargeWholePartNumber")]
    [TestCase(31, 30, true, "0.922337203685477580711", ExpectedResult = true, TestName = "VeryLargeFractionalPartNumber")]
    public bool IsValidNumber_WithValidArguments_ReturnsTrue(int precision, int scale, bool onlyPositive, string value)
    {
        var numberValidator = new NumberValidator(precision, scale, onlyPositive);
        
        return numberValidator.IsValidNumber(value);
    }
    
    [TestCase(3, 2, true, "-0.00", ExpectedResult = false, TestName = "PrecisionLessThanValueLength")]
    [TestCase(30, 2, true, "0.922", ExpectedResult = false, TestName = "FractionalPartNumberGreaterScale")]
    [TestCase(30, 2, true, "a.sd", ExpectedResult = false, TestName = "Letters")]
    [TestCase(30, 2, true, "1.sd", ExpectedResult = false, TestName = "MixedNumericAndLetters")]
    [TestCase(30, 2, true, "1.\n", ExpectedResult = false, TestName = "MixedNumericAndControlCharacter")]
    [TestCase(4, 2, true, "+\n.10", ExpectedResult = false, TestName = "MixedNumericAndPositiveLeadingControlCharacter")]
    [TestCase(30, 2, true, "🛠.🛠🛠", ExpectedResult = false, TestName = "HTMLSymbols")]
    [TestCase(30, 2, true, "", ExpectedResult = false, TestName = "EmptyValue")]
    [TestCase(30, 2, true, null, ExpectedResult = false, TestName = "NullValue")]
    [TestCase(30, 2, true, "Hello world!", ExpectedResult = false, TestName = "Text")]
    [TestCase(30, 2, true, "0+0", ExpectedResult = false, TestName = "InvalidSignPosition")]
    [TestCase(4, 2, true, "+-0.0", ExpectedResult = false, TestName = "DuplicateSigns")]
    [TestCase(30, 2, true, "+++", ExpectedResult = false, TestName = "MultipleSigns")]
    [TestCase(30, 2, true, "   ", ExpectedResult = false, TestName = "WhitespaceOnly")]
    [TestCase(30, 2, true, ".12", ExpectedResult = false, TestName = "MissingIntegerPart")]
    public bool IsValidNumber_WithInvalidArguments_ReturnsFalse(int precision, int scale, bool onlyPositive, string value)
    {   
        var numberValidator = new NumberValidator(precision, scale, onlyPositive);
        
        return numberValidator.IsValidNumber(value);
    }
}