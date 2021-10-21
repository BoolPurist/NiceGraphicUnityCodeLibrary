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
  [TestFixture]
  public class TestRigidInterpolatedMovement
  {
    private RigidInterpolatedMovement _componentToTest;

    [SetUp]
    public void ConstructObject()
      => _componentToTest = TestBase_RigidMotion.SetUp<RigidInterpolatedMovement>();

    [TearDown]
    public void DeConstructObject()
      => TestBase_RigidMotion.TearDown(_componentToTest.gameObject);

    [Test]
    public void Test_MovementGlobal()
      => TestBase_RigidMotion.TestRun_HasMovedGlobal(_componentToTest);

    [Test]
    public void Test_MovementLocal()
      => TestBase_RigidMotion.TestRun_HasMovedLocal(_componentToTest);

    private const float TEST_DURATION_GROWING_SPEED = 10f;
    private const float TEST_DELTA_STEP_GROWING_SPEED = 1f;

    [Test]
    public void Test_GrowingMovementGobal()
      => TestBase_RigidMotion.TestRun_ForInterpolatedMotionGrowing(
        _componentToTest, 
        MovementAxisLevel.Global,
        TEST_DURATION_GROWING_SPEED, 
        TEST_DELTA_STEP_GROWING_SPEED
        );

    [Test]
    public void Test_GrowingMovementLocal()
      => TestBase_RigidMotion.TestRun_ForInterpolatedMotionGrowing(
        _componentToTest,
        MovementAxisLevel.Local,
        TEST_DURATION_GROWING_SPEED, 
        TEST_DELTA_STEP_GROWING_SPEED
        );

    [Test]
    public void Test_SlowingMovementGobal()
      => TestBase_RigidMotion.Test_RunForInterpolatedMotionSlowing(
        _componentToTest, 
        MovementAxisLevel.Global,
        TEST_DURATION_GROWING_SPEED, 
        TEST_DELTA_STEP_GROWING_SPEED
        );

    [Test]
    public void Test_SlowingMovementLocal()
      => TestBase_RigidMotion.Test_RunForInterpolatedMotionSlowing(
        _componentToTest,
        MovementAxisLevel.Local,
        TEST_DURATION_GROWING_SPEED,
        TEST_DELTA_STEP_GROWING_SPEED
        );




  }
}