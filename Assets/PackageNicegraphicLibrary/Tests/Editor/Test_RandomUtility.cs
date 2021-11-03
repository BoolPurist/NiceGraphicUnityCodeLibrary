using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using NiceGraphicLibrary.Utility;
using NiceGraphicLibrary.Tests.Editor.Stubs;

namespace NiceGraphicLibrary.Tests.Editor
{
  [TestFixture]
  public class Test_RandomUtility
  {
    private FakeRandomGenerator _fakeRandomGenerator;

    [SetUp]
    public void SetUp()
    {
      RandomUtility.SetRandomGenerator(new UnityRandomGenerator());
      _fakeRandomGenerator = new FakeRandomGenerator();
    }   

    [TestCaseSource(nameof(TestCases_NullOrEmpty))]
    public void Test_PickRandomFrom_ShouldReturnDefault(int [] input)
    {
      int actualReturnValue = RandomUtility.PickRandomFrom(input);
      int expectedReturnValue = default;

      Assert.AreEqual(
        actualReturnValue, 
        expectedReturnValue, 
        $"For null or empty array the default value should have been returned {expectedReturnValue}"
        );      
    }

    [TestCaseSource(nameof(TestCases_SomeArrays))]
    public void Test_PickRandomFrom_ShouldReturnRandomValue(int[] input, int numberOfInvocations)
    {
      // Set up
      int length = input.Length;
      var cachedSortedInputArry = new int[length];
      Array.Copy(input, cachedSortedInputArry, length);
      Array.Sort(cachedSortedInputArry);

      // Act
      int[] expectedRandomValues = CreateExpectedRandomArray(out int[] indexesToFake);
      RandomUtility.SetRandomGenerator(_fakeRandomGenerator);
      int[] actualRandomValues = CreateActualRandomArray(indexesToFake);

      // Assert
      Assert.AreEqual(
        expectedRandomValues, 
        actualRandomValues, 
        $"Random value are not as expected with test seed"
        );

      Array.Sort(input);
      Assert.AreEqual(
        cachedSortedInputArry, 
        input,
        $"Content of some element was changed"
        );

      int[] CreateExpectedRandomArray(out int[] randomIndexes)
      {
        var expectedValues = new List<int>();
        randomIndexes = new int[numberOfInvocations];

        for (int i = 0; i < numberOfInvocations; i++)
        {
          int randomIndex = UnityEngine.Random.Range(0, input.Length);
          randomIndexes[i] = randomIndex;
          int randomValue = input[randomIndex];
          expectedValues.Add(randomValue);
        }
        
        return expectedValues.ToArray();
      }

      int[] CreateActualRandomArray(int[] indexesToFake)
      {
        var actualValues = new List<int>();

        for (int i = 0; i < numberOfInvocations; i++)
        {
          _fakeRandomGenerator.FakeRangeReturnValue = indexesToFake[i];
          actualValues.Add(RandomUtility.PickRandomFrom(input));
        }
        
        return actualValues.ToArray();

      }
    }

    [TestCaseSource(nameof(TestCases_DoesChanceOccure))]
    public void Test_DoesChanceOccure(
      float acutalGivenProbability,
      float actualGivenTotal,
      float actualFakeRandomValue,
      bool expectedReturnValue
      )
    {
      RandomUtility.SetRandomGenerator(_fakeRandomGenerator);
      _fakeRandomGenerator.FakeValue = actualFakeRandomValue;
      bool actualReturnValue = RandomUtility.DoesChanceOccure(acutalGivenProbability, actualGivenTotal);
      Assert.AreEqual(
        expectedReturnValue, 
        actualReturnValue, 
        $"Actual given probability {acutalGivenProbability} " +
        $"Random fake value {actualFakeRandomValue}"
        );
    }

    #region Test cases
    public static object[] TestCases_DoesChanceOccure
      => new object[]
      {
        new object[]
        {
          // actual given probability
          0.4f,
          // actual given total
          1f,
          // actual fake random value
          0.5f,
          // expected return value
          false
        },
        new object[]
        {
          // actual given probability
          0.8f,
          // actual given total
          1f,
          // actual fake random value
          0.2f,
          // expected return value
          true
        },
        new object[]
        {
          // actual given probability
          1.4f,
          // actual given total
          2f,
          // actual fake random value
          0.5f,
          // expected return value
          true
        },
        new object[]
        {
          // actual given probability
          0.8f,
          // actual given total
          2f,
          // actual fake random value
          0.5f,
          // expected return value
          false
        },
        // Negative values should be treated as their absolute values
        new object[]
        {
          // actual given probability
          -0.8f,
          // actual given total
          -2f,
          // actual fake random value
          0.5f,
          // expected return value
          false
        }
      };



    public static object[] TestCases_SomeArrays
      => new object[]
      {
        new object[] 
        {
          Enumerable.Range(1, 20).ToArray(),
          10
        },
        new object[]
        {
          Enumerable.Range(1, 2).ToArray(),
          20
        },
        new object[]
        {
          new int[] { 2, -8, 9, 78, 456 },
          3
        }
      };

    public static object[] TestCases_NullOrEmpty
      => new object[] { null, new int[] { } };

    #endregion
  }
}