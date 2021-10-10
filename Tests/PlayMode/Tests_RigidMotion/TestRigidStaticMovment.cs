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
  public class TestRigidStaticMovment
  {

    
    private RigidStaticMovment _componentToTest;

    [SetUp]
    public void ConstructObject()
    {
      _componentToTest = TestBase_RigidMotion.SetUp<RigidStaticMovment>();
    }

    [TearDown]
    public void DeConstructObject()
    {
      TestBase_RigidMotion.TearDown(_componentToTest.gameObject);
    }

    [Test]
    public void Test_Movement()
    {      
      TestBase_RigidMotion.Test_HasMoved(_componentToTest);
    }
  }
}