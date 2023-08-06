namespace Application.Common.Constants;

/// <summary>
/// Sizes to be used for Validation (but not for database configuration)
/// </summary>
public static class DefaultEmailConstants
{
    public static string GetHeader(string OrganizationName)
    {
        string pathsvg = Path.Combine(Environment.CurrentDirectory + "\\wwwroot\\favicon.ico");
        byte[] bytes = System.IO.File.ReadAllBytes(pathsvg);
        var imageBase64 = "data:image/png;base64," + Convert.ToBase64String(bytes);

        return @$"<div style=""
                  box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
                  transition: 0.3s;
                  width: 40%;
                  padding: 1px 10px;
                  border-radius: 5px;
                  border: 6px solid #dedede;
                  font-family:Verdana,Geneva,sans-serif;""><h2 style=""margin-left:20px""><strong>{OrganizationName}</strong>  <span style=""color:#999999"">|</span> <img style=""height:31px; width:50px"" alt=""logo"" src=""{imageBase64}""  /> </h2>";

    }
    public static string GetFooter()
    {
        return @$"<br/><br/>
            <span style=""color:#999999"">&copy; Conic ISV All rights reserved {DateTime.Now.Year}. &copy; {DateTime.Now.Year}</span></p></div>";

    }

}

