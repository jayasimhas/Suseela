package com.velir;

import com.velir.baseclass.SetupClass;
import com.velir.pageobject.WhiteBoardPage;
import org.openqa.selenium.By;
import org.testng.Assert;
import org.testng.annotations.Test;

/**
 * Created by ishan.kumar on 8/17/2016.
 */
public class BackendSecurity extends SetupClass {

    private void navigateAndLogin() {
        driver.get(ENV_BE + "/login?returnUrl=/vwb");
        WhiteBoardPage page = new WhiteBoardPage(driver);
        page.boardLogin(configuration.getString("Auth.username"),configuration.getString("Auth.password"));
    }


    @Test
    public void singleSignon(){
        By fileLocator = By.xpath(".//*[@id='tblResults']/tbody/tr[3]/td[1]/a");

        navigateAndLogin();

        helper.click(fileLocator);
        Assert.assertTrue(driver.findElement(fileLocator).getAttribute("href").contains(".docx?sc_mode=preview"));

    }
}
