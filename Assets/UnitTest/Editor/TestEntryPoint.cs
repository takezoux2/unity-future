using System;

using SharpUnit;
using System.Collections;
using System.Reflection;
using UnityEditor;


/// <summary>
/// Support to run test from console.
/// 
/// To run test, execute from console at project root directory.
/// <pre>
/// /Applications/Unity/Unity.app/Contents/MacOS/Unity -quit -batchmode -executeMethod TestEntryPoint.RunAllTests -logfile TestReports/log.txt
/// </pre>
/// Test reports are exported to {project_root}/TestReports directory.
/// 
/// </summary>
public class TestEntryPoint
{
	
	public static void RunAllTests(){
		
        // Create test suite
        TestSuite suite = new TestSuite();
        // For each assembly in this app domain
        foreach (Assembly assem in AppDomain.CurrentDomain.GetAssemblies())
        {
            // For each type in the assembly
            foreach (Type type in assem.GetTypes())
            {
                // If this is a valid test case
                // i.e. derived from TestCase and instantiable
                if (typeof(TestCase).IsAssignableFrom(type) &&
                    type != typeof(TestCase) &&
                    !type.IsAbstract)
                {
                    // Add tests to suite
                    suite.AddAll(type.GetConstructor(new Type[0]).Invoke(new object[0]) as TestCase);
                }
            }
        }

        // Run the tests
        TestResult res = suite.Run(null);
		
		
		var xmlReporter = new XML_TestReporter();
		if(!System.IO.Directory.Exists("TestReports")){
			System.IO.Directory.CreateDirectory("TestReports");
		}
		xmlReporter.Init("TestReports/test-all.xml");
		xmlReporter.LogResults(res);
		if(res.NumFailed > 0){
			EditorApplication.Exit(10);
		}
	}
	
	public static void ConsoleCheck(){
		
		Console.Write("Run !");
	}
	
	public TestEntryPoint ()
	{
	}
}

