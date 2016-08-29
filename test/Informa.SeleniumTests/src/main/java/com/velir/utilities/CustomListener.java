package com.velir.utilities;

import com.gurock.testrail.APIClient;
import org.apache.commons.configuration.PropertiesConfiguration;
import org.json.simple.JSONObject;
import org.testng.ITestContext;
import org.testng.ITestResult;
import org.testng.TestListenerAdapter;

import java.util.HashMap;
import java.util.Map;

public class CustomListener extends TestListenerAdapter{
    private int m_count = 0;
    PropertiesConfiguration configuration;
    private static APIClient client;
    private static String testRun;
   // public



    @Override
    public void onStart(ITestContext context){
        try {
            testRailSetup();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    @Override
    public void onTestFailure(ITestResult tr) {
        try {

           resultWriter(configuration.getString(tr.getTestName().toString()), "5", "QA Failed");}
        catch(Exception e){e.printStackTrace();}
        log(tr.getName()+ "--Test method failed\n");
    }

    @Override
    public void onTestSkipped(ITestResult tr) {
        try {
           // testRailSetup();
           resultWriter(configuration.getString(tr.getTestName().toString()), "3", "Retest");}
        catch(Exception e){e.printStackTrace();}
        log(tr.getName()+ "--Test method skipped\n");
    }

    @Override
    public void onTestSuccess(ITestResult tr){
        try {
    System.out.println(configuration.getString(tr.getName()));
        resultWriter(configuration.getString(tr.getName()), "1", "QA Passed");}
    catch(Exception e){e.printStackTrace();}
        log(tr.getName() + "--Test method success\n");
    }

    private void log(String string) {
        System.out.print(string);
        if (++m_count % 40 == 0) {
            System.out.println("");
        }
    }



    public static void testRailLogin(String testRunID) throws Exception{
        client = new APIClient("https://velir.testrail.com/");
        testRun = testRunID;
        client.setUser("ishan.kumar@velir.com");
        client.setPassword("velir123");
    }

    public void testRailSetup() throws Exception{
        configuration = new PropertiesConfiguration("testdata/testrail.properties");
        testRailLogin(configuration.getString("testRunID"));
        System.out.println(configuration.getString("testRunID"));
    }


    public static void resultWriter(String testCaseID, String testResultID, String comment) throws Exception
    {
        Map data = new HashMap();
        data.put("status_id", new Integer(testResultID));
        data.put("comment", comment);
        JSONObject r = (JSONObject) client.sendPost("add_result_for_case/"+testRun+"/"+testCaseID, data);

    }


}