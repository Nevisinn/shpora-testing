using FluentAssertions;
using FluentAssertions.Equivalency;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.ObjectComparison;
public class ObjectComparison
{
    [Test]
    [Description("Проверка текущего царя")]
    [Category("ToRefactor")]
    public void CheckCurrentTsar()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));
        
        //Fluent Assertions
        actualTsar.Should()
            .BeEquivalentTo(expectedTsar, options => options
                .Excluding((IMemberInfo memberInfo) => memberInfo.Name == "Id"));
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));
        
        // Какие недостатки у такого подхода?
        /*
        Недостатки: 
        1. При падении теста нет возможности отследить причину, так как вывод теста это True либо False
        2. Нет обработки рекурсии, приложение может упасть с ошибкой переполнения стека при циклических ссылках Parent.
        3. При измении Person (удаление/добавление свойств), придется модифицировать метод 
        
        Мое решение лучше тем, что:  
        1. В случае не прохождения теста, выводит все причины 
        2. Отсутствие дублирования кода
        3. Обработка рекурсии под капотом: рекурсия выполняется до 10 уровней, далее тест падает с ошибкой, что есть 
        циклическая зависимость 
        4. Person расширяем: нет необходимости в модификации метода
        */
        ClassicAssert.True(AreEqual(actualTsar, expectedTsar));
    }

    private bool AreEqual(Person? actual, Person? expected)
    {
        if (actual == expected) return true;
        if (actual == null || expected == null) return false;
        return
            actual.Name == expected.Name
            && actual.Age == expected.Age
            && actual.Height == expected.Height
            && actual.Weight == expected.Weight
            && AreEqual(actual.Parent, expected.Parent);
    }
}
