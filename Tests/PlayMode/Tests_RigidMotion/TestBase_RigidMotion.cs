using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using NiceGraphicLibrary;
using NiceGraphicLibrary.Component;

namespace NiceGraphicLibrary.PlayTests
{

  
  
  public static class TestBase_RigidMotion
  {
    private const float TEST_SPEED = 10f;
    private const float DELTA_TIME_VALUE = 1f;

    private enum TestMovementDirection
    {
      Left,
      Right,
      Up,
      Down,
      Forward,
      Backward
    }

    public static T SetUp<T>() where T : RigidGeometryMotion
    {
      GameObject objectToTest = new GameObject("Object to test");
      T componentToTest = objectToTest.AddComponent<T>();
      var rb = componentToTest.GetComponent<Rigidbody>();
      rb.useGravity = false;
      componentToTest.SetSpeed(TEST_SPEED);

      var fakeDeltaTime = new FakeDeltaTimeProvider(DELTA_TIME_VALUE);
      componentToTest.ProvideDeltaTime(fakeDeltaTime);

      return componentToTest;
    }

    public static void TearDown(GameObject objectToDestroy)
    {
      GameObject.Destroy(objectToDestroy);
    }

    public static void Test_HasMoved(RigidGeometryMotion componentToTest)
    {      
      if (componentToTest == null)
      {
        throw new ArgumentNullException($"{nameof(componentToTest)} must not be null");
      }

      Transform objectLocation = componentToTest.gameObject.transform;

      // Testing movement in x Axis left
      TestStep(TestMovementDirection.Left);
      TestStep(TestMovementDirection.Right);
      TestStep(TestMovementDirection.Up);
      TestStep(TestMovementDirection.Down);
      TestStep(TestMovementDirection.Forward);
      TestStep(TestMovementDirection.Backward);

      void TestStep(TestMovementDirection testDirection)
      {
        objectLocation.position = Vector3.zero;
        
        float movementInChoosenAxis = 0f;
        float movementDirection = 1f;
        string nameOfAxis = "";
        string nameOfDirection = "";

        switch (testDirection)
        {
          case TestMovementDirection.Left:
            componentToTest.OnXMotion(1f);
            componentToTest.ExecuteNextFixedUpdate();
            movementInChoosenAxis = objectLocation.position.x;
            nameOfAxis = "X";
            nameOfDirection = "left";
            break;
          case TestMovementDirection.Right:
            componentToTest.OnXMotion(-1f);
            componentToTest.ExecuteNextFixedUpdate();
            movementInChoosenAxis = objectLocation.position.x;
            nameOfAxis = "X";
            movementDirection *= -1f;
            nameOfDirection = "right";
            break;
          case TestMovementDirection.Up:
            componentToTest.OnYMotion(1f);
            componentToTest.ExecuteNextFixedUpdate();
            movementInChoosenAxis = objectLocation.position.y;
            nameOfAxis = "Y";
            nameOfDirection = "up";
            break;
          case TestMovementDirection.Down:
            componentToTest.OnYMotion(-1f);
            componentToTest.ExecuteNextFixedUpdate();
            movementInChoosenAxis = objectLocation.position.y;
            nameOfAxis = "Y";
            movementDirection *= -1f;
            nameOfDirection = "down";
            break;
          case TestMovementDirection.Forward:
            componentToTest.OnZMotion(1f);
            componentToTest.ExecuteNextFixedUpdate();
            movementInChoosenAxis = objectLocation.position.z;
            nameOfAxis = "Z";
            nameOfDirection = "forward";
            break;
          case TestMovementDirection.Backward:
            componentToTest.OnZMotion(-1f);
            componentToTest.ExecuteNextFixedUpdate();
            movementInChoosenAxis = objectLocation.position.z;
            nameOfAxis = "Z";
            movementDirection *= -1f;
            nameOfDirection = "backward";
            break;
          default:
            throw new ArgumentException($"{nameof(testDirection)} has no value know to the switch statement");
        }

        float expectedCoordinate = TEST_SPEED * DELTA_TIME_VALUE * movementDirection;
        Assert.AreEqual(
          movementInChoosenAxis,
          expectedCoordinate,
           $"Object should have been moved {nameOfDirection}, actual position in {nameOfAxis} axis {movementInChoosenAxis}"
          );

      }


      
    }
  }
}