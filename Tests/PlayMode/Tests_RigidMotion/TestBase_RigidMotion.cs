
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools.Utils;

using NiceGraphicLibrary.Component;
using NiceGraphicLibrary.Utility;


namespace NiceGraphicLibrary.PlayTests
{
  public static class TestBase_RigidMotion
  {
    private const float TEST_SPEED = 10f;
    private const float DELTA_TIME_VALUE = 1f;
    private const float GREATER_ERROR_TOLERANCE = 0.0001f;
    private const float EQUAL_ERROR_TOLERANCE = 0.0001f;

    private static readonly Quaternion _testRotation = Quaternion.Euler(20f, 60f, 80f);

    private enum TestMovementDirection
    {
      Left,
      Right,
      Up,
      Down,
      Forward,
      Backward
    }

    private static readonly Dictionary<TestMovementDirection, Vector3> DirectionsGlobal
      = new Dictionary<TestMovementDirection, Vector3>()
      {
        { TestMovementDirection.Left, Vector3.left },
        { TestMovementDirection.Right, Vector3.right },
        { TestMovementDirection.Up, Vector3.up },
        { TestMovementDirection.Down, Vector3.down },
        { TestMovementDirection.Forward, Vector3.forward },
        { TestMovementDirection.Backward, Vector3.back }
      };

    #region Setup and TearDown

    public static T SetUp<T>() where T : RigidGeometryMotion
    {
      GameObject objectToTest = new GameObject("Object to test");
      T componentToTest = objectToTest.AddComponent<T>();
      componentToTest.IsSetForUnitTest = true;
      var rb = componentToTest.GetComponent<Rigidbody>();
      rb.useGravity = false;
      componentToTest.SetSpeed(TEST_SPEED);

      var fakeDeltaTime = new FakeDeltaTimeProvider(DELTA_TIME_VALUE);
      componentToTest.ProvideDeltaTimeWith(fakeDeltaTime);

      Time.timeScale = 0f;
      Time.fixedDeltaTime = 0f;

      return componentToTest;
    }

    public static void TearDown(GameObject objectToDestroy)
    {
      GameObject.Destroy(objectToDestroy);
      Time.timeScale = 1f;
      Time.fixedDeltaTime = 1f;
    }

    #endregion

    #region Test runs which test every direction right, left, up, down, front and back.
    public static void TestRun_HasMovedForStaticMovementGlobal(RigidStaticMovment componentToTest)
    {      
      if (componentToTest == null)
      {
        throw new ArgumentNullException($"{nameof(componentToTest)} must not be null");
      }
            
      TestStep_StaticMovement(componentToTest, TestMovementDirection.Left, DirectionsGlobal);
      TestStep_StaticMovement(componentToTest, TestMovementDirection.Right, DirectionsGlobal);
      TestStep_StaticMovement(componentToTest, TestMovementDirection.Up, DirectionsGlobal);
      TestStep_StaticMovement(componentToTest, TestMovementDirection.Down, DirectionsGlobal);
      TestStep_StaticMovement(componentToTest, TestMovementDirection.Forward, DirectionsGlobal);
      TestStep_StaticMovement(componentToTest, TestMovementDirection.Backward, DirectionsGlobal);    
    }

    public static void TestRun_HasMovedForStaticMovementLocal(
      RigidStaticMovment componentToTest
      )
    {
      Dictionary<TestMovementDirection, Vector3> localDirection = SetUpComponentForTestRun(componentToTest, MovementAxisLevel.Local);

      TestStep_StaticMovement(componentToTest, TestMovementDirection.Left, localDirection);
      TestStep_StaticMovement(componentToTest, TestMovementDirection.Right, localDirection);
      TestStep_StaticMovement(componentToTest, TestMovementDirection.Up, localDirection);
      TestStep_StaticMovement(componentToTest, TestMovementDirection.Down, localDirection);
      TestStep_StaticMovement(componentToTest, TestMovementDirection.Forward, localDirection);
      TestStep_StaticMovement(componentToTest, TestMovementDirection.Backward, localDirection);      
    }

    public static void TestRun_HasMovedGlobal(RigidGeometryMotion movementComponentToTest)
    {
      movementComponentToTest.AxisLevel = MovementAxisLevel.Global;

      TestStep_HasMovedGlobal(movementComponentToTest, TestMovementDirection.Right);
      TestStep_HasMovedGlobal(movementComponentToTest, TestMovementDirection.Left);
      TestStep_HasMovedGlobal(movementComponentToTest, TestMovementDirection.Up);
      TestStep_HasMovedGlobal(movementComponentToTest, TestMovementDirection.Down);
      TestStep_HasMovedGlobal(movementComponentToTest, TestMovementDirection.Forward);
      TestStep_HasMovedGlobal(movementComponentToTest, TestMovementDirection.Backward);
    }

    public static void TestRun_HasMovedLocal(RigidGeometryMotion movementComponentToTest)
    {

      Dictionary<TestMovementDirection, Vector3> directions = SetUpComponentForTestRun(movementComponentToTest, MovementAxisLevel.Global);

      TestStep_HasMovedLocal(movementComponentToTest, TestMovementDirection.Right, directions);
      TestStep_HasMovedLocal(movementComponentToTest, TestMovementDirection.Left, directions);
      TestStep_HasMovedLocal(movementComponentToTest, TestMovementDirection.Up, directions);
      TestStep_HasMovedLocal(movementComponentToTest, TestMovementDirection.Down, directions);
      TestStep_HasMovedLocal(movementComponentToTest, TestMovementDirection.Forward, directions);
      TestStep_HasMovedLocal(movementComponentToTest, TestMovementDirection.Backward, directions);
    }

    public static void Test_RunForInterpolatedMotionSlowing(
      RigidInterpolatedMotion componentToTest,
      MovementAxisLevel localOrGlobal,
      in float duration,
      in float durationStep
      )
    {
      Dictionary<TestMovementDirection, Vector3> directions = SetUpComponentForTestRun(componentToTest, localOrGlobal);

      TestStep_IsMovingSlowinDown(componentToTest, duration, durationStep, TestMovementDirection.Right, directions);
      TestStep_IsMovingSlowinDown(componentToTest, duration, durationStep, TestMovementDirection.Left, directions);
      TestStep_IsMovingSlowinDown(componentToTest, duration, durationStep, TestMovementDirection.Up, directions);
      TestStep_IsMovingSlowinDown(componentToTest, duration, durationStep, TestMovementDirection.Down, directions);
      TestStep_IsMovingSlowinDown(componentToTest, duration, durationStep, TestMovementDirection.Forward, directions);
      TestStep_IsMovingSlowinDown(componentToTest, duration, durationStep, TestMovementDirection.Backward, directions);
    }

    public static void TestRun_ForInterpolatedMotionGrowing(
      RigidInterpolatedMotion componentToTest,
      MovementAxisLevel localOrGlobal,
      in float duration,
      in float durationStep
      )
    {

      Dictionary<TestMovementDirection, Vector3> directions = SetUpComponentForTestRun(
        componentToTest,
        localOrGlobal
        );

      TestStep_IsMovingGrowing(componentToTest, duration, durationStep, TestMovementDirection.Right, directions);
      TestStep_IsMovingGrowing(componentToTest, duration, durationStep, TestMovementDirection.Left, directions);
      TestStep_IsMovingGrowing(componentToTest, duration, durationStep, TestMovementDirection.Up, directions);
      TestStep_IsMovingGrowing(componentToTest, duration, durationStep, TestMovementDirection.Down, directions);
      TestStep_IsMovingGrowing(componentToTest, duration, durationStep, TestMovementDirection.Forward, directions);
      TestStep_IsMovingGrowing(componentToTest, duration, durationStep, TestMovementDirection.Backward, directions);
    }

    #endregion

    #region Test steps execute a test for one given direction

    private static void TestStep_StaticMovement(
      RigidGeometryMotion movementComponentToTest, 
      TestMovementDirection testDirection,
      Dictionary<TestMovementDirection, Vector3> directions
      )
    {

      string nameOfDirection, nameOfAxis;

      Vector3 expectedPosition = GetPositionAfterMovement(
        out nameOfDirection, 
        out nameOfAxis,
        movementComponentToTest,
        testDirection,
        directions
        );

      Assert.AreEqual(
          expectedPosition,
          movementComponentToTest.gameObject.transform.position,
           $"Object should have been moved in direction {nameOfDirection}, actual position in {nameOfAxis} axis {expectedPosition}"
        );

    }

    private static void TestStep_HasMovedGlobal(
      RigidGeometryMotion movementComponentToTest,
      TestMovementDirection testDirection
      )
    {
      string nameOfDirection, nameOfAxis;

      Vector3 actualPosition = GetPositionAfterMovement(
        out nameOfDirection,
        out nameOfAxis,
        movementComponentToTest,
        testDirection,
        DirectionsGlobal
        );

      switch (testDirection)
      {
        case TestMovementDirection.Right:
          Assert.Greater(
            actualPosition.x,
            0f,
            GetErrorMessage(false, nameOfAxis)
            );
          AssertForYAtZero();
          AssertForZAtZero();
          break;
        case TestMovementDirection.Left:
          Assert.Less(
            actualPosition.x,
            0f,
            GetErrorMessage(false, nameOfAxis)
            );
          AssertForYAtZero();
          AssertForZAtZero();
          break;
        case TestMovementDirection.Up:
          Assert.Greater(
            actualPosition.y,
            0f,
            GetErrorMessage(false, nameOfAxis)
            );
          AssertForXAtZero();
          AssertForZAtZero();
          break;
        case TestMovementDirection.Down:
          Assert.Less(
            actualPosition.y,
            0f,
            GetErrorMessage(false, nameOfAxis)
            );
          AssertForXAtZero();
          AssertForZAtZero();
          break;
        case TestMovementDirection.Forward:
          Assert.Greater(
            actualPosition.z,
            0f,
            GetErrorMessage(false, nameOfAxis)
            );
          AssertForXAtZero();
          AssertForYAtZero();
          break;
        case TestMovementDirection.Backward:
          Assert.Less(
            actualPosition.z,
            0f,
            GetErrorMessage(false, nameOfAxis)
            );
          AssertForXAtZero();
          AssertForYAtZero();
          break;
        default:
          throw new ArgumentException($"Switch  does not consider {testDirection} as Value for enum {nameof(TestMovementDirection)}");
      }


      string GetErrorMessage(in bool shouldRemainAtZero, in string axisName)
      {
        string errorMessage = $"The object should have been moved in direction {nameOfDirection}.";
        if (shouldRemainAtZero)
        {
          errorMessage += $"Position should have been remained at {axisName} coordinate.";
        }
        else
        {
          errorMessage += $"Position should have been changed in {axisName} coordinate. Actual position is {actualPosition}";
        }

        errorMessage += $"Actual position is { actualPosition }";

        return errorMessage;
      }

      void AssertForXAtZero()
      {
        Assert.AreEqual(
          actualPosition.x,
          0f,
          GetErrorMessage(true, "X")
          );
      }

      void AssertForYAtZero()
      {
        Assert.AreEqual(
          actualPosition.y,
          0f,
          GetErrorMessage(true, "Y")
          );
      }

      void AssertForZAtZero()
      {
        Assert.AreEqual(
          actualPosition.z,
          0f,
          GetErrorMessage(true, "Z")
          );
      }
    }

    private static void TestStep_HasMovedLocal(
      RigidGeometryMotion movementComponentToTest,
      in TestMovementDirection testDirection,
      Dictionary<TestMovementDirection, Vector3> localDirections
      )
    {
      Vector3 targetDirection = localDirections[testDirection];
      Transform objectLocation = ResetMotionComponentForMoveAndPosition(movementComponentToTest);

      Action movementActionToPerform = GetCallBackForTestDirection(movementComponentToTest, testDirection);

      movementActionToPerform();
      movementComponentToTest.ExecuteNextFixedUpdate();

      Vector3 actualPointAfterMovement = objectLocation.position;
      Assert.Greater(
        actualPointAfterMovement.sqrMagnitude,
        Vector3.zero.sqrMagnitude,
        $"Object was not moved at all."
        );
      Assert.IsTrue(
        Geometry3DUtility.IsPointInDirection(actualPointAfterMovement, targetDirection),
        $"Actual position {actualPointAfterMovement}, rotation of object {objectLocation.eulerAngles}, actual direction {testDirection}, direction vector {targetDirection}"
        );
    }

    private static void TestStep_IsMovingGrowing(
      RigidInterpolatedMotion componentToTest,
      float duration,
      float deltaStep,
      TestMovementDirection testDirection,
      Dictionary<TestMovementDirection, Vector3> directionDictionary
      )
    {
      AttachFakeDeltaTimeProvider(componentToTest, deltaStep);
      componentToTest.Duration = duration;

      Transform objectTransform = ResetMotionComponentForMoveAndPosition(componentToTest);
      Action testCallBackForMovement = GetCallBackForTestDirection(componentToTest, testDirection);
      Func<float, float, bool> coordinateChecker = GetCallBackForDirectionCheck();
      Func<RigidInterpolatedMotion, MovingState> getterForActualMovingState = GetterForMovingState(testDirection);
      MovingState expectedMovingStateEveryFrame = GetExpectedMovingState(testDirection, false);

      float previousDeltaDistance = 0f;

      for (float passedTime = 0f; passedTime < duration; passedTime += deltaStep)
      {
        float currentDeltaDistance = 0f;


        Vector3 previousCoordinate = objectTransform.position;

        testCallBackForMovement();
        componentToTest.ExecuteNextFixedUpdate();

        Vector3 currentCoordinate = objectTransform.position;

        // Have to round up result 
        currentDeltaDistance = Vector3.Distance(previousCoordinate, currentCoordinate);
        float currentDeltaDistanceToTest = currentDeltaDistance + GREATER_ERROR_TOLERANCE;

        Assert.True(
          Geometry3DUtility.IsPointInDirection(objectTransform.position, directionDictionary[testDirection]),
          $"Object did not move further into the direction {testDirection}." +
          $"Object moved from {objectTransform.position} to {objectTransform.position}"
          );

        Assert.GreaterOrEqual(
            currentDeltaDistanceToTest,
            previousDeltaDistance,
            $"Object did not travel a larger distance compared with the previous distance in direction {testDirection}. " +
            $"Last traveled distance {currentDeltaDistance} and previous traveled distance {previousCoordinate}"
          );

        MovingState actualState = getterForActualMovingState(componentToTest);

        AssertForMovingState(actualState, expectedMovingStateEveryFrame);

        previousDeltaDistance = currentDeltaDistance;
      }



      Func<float, float, bool> GetCallBackForDirectionCheck()
      {
        switch (testDirection)
        {
          case TestMovementDirection.Right:
          case TestMovementDirection.Up:
          case TestMovementDirection.Forward:
            return (currentCoordinate, previousCoordinate) => currentCoordinate > previousCoordinate;
          case TestMovementDirection.Left:
          case TestMovementDirection.Down:
          case TestMovementDirection.Backward:
            return (currentCoordinate, previousCoordinate) => currentCoordinate < previousCoordinate;
          default:
            throw new ArgumentException($"For the Value {testDirection} of the enum{nameof(TestMovementDirection)}");
        }
      }


    }

    private static void TestStep_IsMovingSlowinDown(
      RigidInterpolatedMotion componentToTest,
      in float duration,
      in float deltaStep,
      in TestMovementDirection testDirection,
      Dictionary<TestMovementDirection, Vector3> directionDictionary
      )
    {
      const float ONE = 1f;

      // Set up component so it reached its maximum speed for testing 
      Transform objectTransform = ResetMotionComponentForMoveAndPosition(componentToTest);
      AttachFakeDeltaTimeProvider(componentToTest, ONE);
      componentToTest.Duration = ONE;

      Action setUpBeforeSlowingDown = GetCallBackForTestDirection(componentToTest, testDirection);
      setUpBeforeSlowingDown();
      componentToTest.ExecuteNextFixedUpdate();

      AttachFakeDeltaTimeProvider(componentToTest, deltaStep);
      componentToTest.SlowingDuration = duration;
      
      // Running test

      float previousDeltaDistance = float.MaxValue;
      float currentDeltaDistance = 0f;
      Vector3 previousPosition = objectTransform.position;
      Vector3 currentPosition = objectTransform.position;
      MovingState expectedSlowingState = GetExpectedMovingState(testDirection, true);
      Func<RigidInterpolatedMotion, MovingState> getterForCurrentMovingState = GetterForMovingState(testDirection);
      MovingState actualState;

      
      for (float currentSlowingDuration = 0; currentSlowingDuration < duration; currentSlowingDuration += deltaStep)
      {
        componentToTest.ExecuteNextFixedUpdate();

        currentPosition = objectTransform.position;
        currentDeltaDistance = Vector3.Distance(previousPosition, currentPosition);
        float currentDeltaDistanceToTest = currentDeltaDistance + GREATER_ERROR_TOLERANCE;

        Assert.IsTrue(
          Geometry3DUtility.IsPointInDirection(objectTransform.position, directionDictionary[testDirection]),
          $"Object did not move further into the direction {testDirection}." +
          $"Object moved from {previousPosition} to {objectTransform.position}"
          );

        Assert.LessOrEqual(
          currentDeltaDistanceToTest,
          previousDeltaDistance,
          $"Object did slow down ! " + 
          $"Actual distance traveled {currentDeltaDistance} and previous traveled distance {previousDeltaDistance}"
          );
        
        previousDeltaDistance = currentDeltaDistance;
        previousPosition = currentPosition;

        actualState = getterForCurrentMovingState(componentToTest);
        AssertForMovingState(actualState, expectedSlowingState);
      }

      // Last update should result in standing.
      componentToTest.ExecuteNextFixedUpdate();
      actualState = getterForCurrentMovingState(componentToTest);

      AssertForMovingState(actualState, MovingState.Standing);     
    }

    #endregion

    #region Routines
    private static Dictionary<TestMovementDirection, Vector3> SetUpComponentForTestRun(
      RigidGeometryMotion component, 
      MovementAxisLevel axisLevel
      )
    {
      component.AxisLevel = axisLevel;
      if (axisLevel == MovementAxisLevel.Local)
      {
        component.transform.rotation = _testRotation;
      }
      return axisLevel == MovementAxisLevel.Global ?
        DirectionsGlobal : CreateLocalDirectionsLocal(component.transform);
    }

    private static float GetCurrentCoordinateFromDirection(
      in TestMovementDirection testDirection,
      Transform objectWithLocation
      )
    {
      switch (testDirection)
      {
        case TestMovementDirection.Right:
        case TestMovementDirection.Left:
          return objectWithLocation.position.x;
        case TestMovementDirection.Up:
        case TestMovementDirection.Down:
          return objectWithLocation.position.y;
        case TestMovementDirection.Forward:
        case TestMovementDirection.Backward:
          return objectWithLocation.position.z;
        default:
          throw new ArgumentException($"For the Value {testDirection} of the enum {nameof(TestMovementDirection)}");
      }
    }

    private static Func<float, float, bool> GetCallBackForCounterDirectionCheck(in TestMovementDirection testDirection)
    {
      switch (testDirection)
      {
        case TestMovementDirection.Right:
        case TestMovementDirection.Up:
        case TestMovementDirection.Forward:
          return (currentCoordinate, previousCoordinate) => currentCoordinate > previousCoordinate;
        case TestMovementDirection.Left:
        case TestMovementDirection.Down:
        case TestMovementDirection.Backward:
          return (currentCoordinate, previousCoordinate) => currentCoordinate < previousCoordinate;
        default:
          ThrowForNotAccountedTestMovementDirectionValue(testDirection);
          return null;
      }
    }

    private static Action GetCallBackForTestDirection(
      RigidGeometryMotion componentToTest, 
      in TestMovementDirection testDirection
      )
    {
      switch (testDirection)
      {
        case TestMovementDirection.Right:
          return () => componentToTest.OnXMotion(1f);
        case TestMovementDirection.Left:
          return () => componentToTest.OnXMotion(-1f);
        case TestMovementDirection.Up:
          return () => componentToTest.OnYMotion(1f);
        case TestMovementDirection.Down:
          return () => componentToTest.OnYMotion(-1f);
        case TestMovementDirection.Forward:
          return () => componentToTest.OnZMotion(1f);
        case TestMovementDirection.Backward:
          return () => componentToTest.OnZMotion(-1f);
        default:
          ThrowForNotAccountedTestMovementDirectionValue(testDirection);
          return null;
      }
    }

    private static Transform ResetMotionComponentForMoveAndPosition(RigidGeometryMotion componentToTest)
    {
      Transform objectTransform = componentToTest.gameObject.transform;
      objectTransform.position = Vector3.zero;
      componentToTest.ForceStand();
      return objectTransform;
    }

    private static void AttachFakeDeltaTimeProvider(RigidInterpolatedMotion componentToWorkWith,in float deltaValue)
    {
      var fakeDeltaProvider = new FakeDeltaTimeProvider(deltaValue);
      componentToWorkWith.ProvideDeltaTimeWith(fakeDeltaProvider);
    }

    private static Dictionary<TestMovementDirection, Vector3> CreateLocalDirectionsLocal(
      Transform transform
      )
    {
      var returnedDictionary = new Dictionary<TestMovementDirection, Vector3>();

      returnedDictionary.Add(TestMovementDirection.Right, transform.right);
      returnedDictionary.Add(TestMovementDirection.Left, transform.right * -1f);

      returnedDictionary.Add(TestMovementDirection.Up, transform.up);
      returnedDictionary.Add(TestMovementDirection.Down, transform.up * -1f);

      returnedDictionary.Add(TestMovementDirection.Forward, transform.forward);
      returnedDictionary.Add(TestMovementDirection.Backward, transform.forward * -1f);

      return returnedDictionary;
    }

    private static Vector3 GetPositionAfterMovement(
      out string nameOfDirection,
      out string nameOfAxis,
      RigidGeometryMotion movementComponentToTest,
      TestMovementDirection testDirection,
      Dictionary<TestMovementDirection, Vector3> directions
      )
    {

      ResetMotionComponentForMoveAndPosition(movementComponentToTest);
      nameOfAxis = "";
      nameOfDirection = "";

      switch (testDirection)
      {
        case TestMovementDirection.Left:
          movementComponentToTest.OnXMotion(-1f);
          nameOfAxis = "X";
          nameOfDirection = "left";
          break;
        case TestMovementDirection.Right:
          movementComponentToTest.OnXMotion(1f);
          nameOfAxis = "X";
          nameOfDirection = "right";
          break;
        case TestMovementDirection.Up:
          movementComponentToTest.OnYMotion(1f);
          nameOfAxis = "Y";
          nameOfDirection = "up";
          break;
        case TestMovementDirection.Down:
          movementComponentToTest.OnYMotion(-1f);
          nameOfAxis = "Y";
          nameOfDirection = "down";
          break;
        case TestMovementDirection.Forward:
          movementComponentToTest.OnZMotion(1f);
          nameOfAxis = "Z";
          nameOfDirection = "forward";
          break;
        case TestMovementDirection.Backward:
          movementComponentToTest.OnZMotion(-1f);
          nameOfAxis = "Z";
          nameOfDirection = "backward";
          break;
        default:
          ThrowForNotAccountedTestMovementDirectionValue(testDirection);
          return Vector3.zero;
      }

      movementComponentToTest.ExecuteNextFixedUpdate();
      Vector3 movementDirection = directions[testDirection];
      Vector3 expetectedPosition = (TEST_SPEED * DELTA_TIME_VALUE) * movementDirection;

      return expetectedPosition;
    }

    private static Func<RigidInterpolatedMotion, MovingState> GetterForMovingState(in TestMovementDirection direction)
    {
      switch (direction)
      {
        case TestMovementDirection.Left:
        case TestMovementDirection.Right:
          return component => component.CurrentMovingStateX;          
        case TestMovementDirection.Up:
        case TestMovementDirection.Down:
          return component => component.CurrentMovingStateY;
        case TestMovementDirection.Forward:
        case TestMovementDirection.Backward:
          return component => component.CurrentMovingStateZ;
        default:
          ThrowForNotAccountedTestMovementDirectionValue(direction);
          return null;
      }
    }

    private static void ThrowForNotAccountedTestMovementDirectionValue(in TestMovementDirection invalidDirection)
      => throw new ArgumentException($"For the Value {invalidDirection} of the enum {nameof(TestMovementDirection)}");

    private static MovingState GetExpectedMovingState(
      in TestMovementDirection testDirection,
      in bool isSlowing
      )
    {
      MovingState forwardState = isSlowing ? MovingState.SlowingDownFront: MovingState.SpeedingUpFront;
      MovingState backwardState = isSlowing ? MovingState.SlowingDownBack : MovingState.SpeedingUpBack;

      switch (testDirection)
      {
        case TestMovementDirection.Right:
        case TestMovementDirection.Up:
        case TestMovementDirection.Forward:
          return forwardState;
        case TestMovementDirection.Left:
        case TestMovementDirection.Down:
        case TestMovementDirection.Backward:
          return backwardState;
        default:
          ThrowForNotAccountedTestMovementDirectionValue(testDirection);
          return MovingState.Standing;
      }
    }

    private static void AssertForMovingState(
      in MovingState actualState,
      in MovingState expectedMovingStateEveryFrame
      )
    {
      Assert.AreEqual(
        expectedMovingStateEveryFrame,
        actualState,
        $"Actual state [{actualState}] is not the supposed state [{expectedMovingStateEveryFrame}]"
        );
    }

    #endregion

  }
}
