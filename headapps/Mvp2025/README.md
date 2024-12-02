# MVP Site Rebuild 2025 - Temporary Readme 
This a temporary readme for information about how to run the new MVP site. Once it is live this readme will be delete and its content moved to the main readme in the root of the repo.

### Running the MVP 2025 site
8. Open XM Cloud Deploy application, and navigate to the Environment you want to connect to.
9. Ensure the `Context` toggle is set to `Preview` otherwise you wont see any changes you make till they're published.
10. Click on the Developer Settings tab and make note of the `JSS_EDITING_SECRET` and `SITECORE_EDGE_CONTEXT_ID` values shown.
11. Clone the repository to your local machine.
12. Open the `./headapps/Mvp2025/Mvp2025.sln` solution in Visual Studio.
13. Make a copy the `appsettings.json` file in the `Mvp2025.Site` project, and name it `appsettings.Development.json`.
14. Set the following values in the `Sitecore` section of the `appsettings.json` file:
    - `EdgeContextId` - The `SITECORE_EDGE_CONTEXT_ID` value from step 10.
    - `EditingSecret` - The `JSS_EDITING_SECRET` value from step 10.
15. Run the application from within Visual Studio by hitting F5, or using the dotnet CLI with `dotnet run`.
16. You will now be able to access the application at `https://localhost:5001/`.

### Connecting Pages to your locally running application
> [!NOTE]
> Temporary steps to connect Pages to your locally running application. This will be updated Meta Page Editing mode is supported.

1. Open `./headapps/Mvp2025/Mvp2025.sln` in Visual Studio then Create a local dev tunnel by following this [guide](https://learn.microsoft.com/en-us/connectors/custom-connectors/port-tunneling)
2. Hit F5 to run the application from Visual Studio, ensuring you have enabled your dev tunnel.
3. When the page loads make a note of the URL, it should in the format `https://XXXX.devtunnels.ms/`. If successful you should see a plan white page rendered.
4. Return to the Content Editor
5. Navigate to the `/sitecore/system/Settings/Services/Rendering Hosts/Mvp` item, and set the following values, ensuring you save the changes:
    - `Server side rendering engine endpoint URL` - `https://<<TUNNEL_URL>>/jss-render`
    - `Server side rendering engine application URL` - `https://<<TUNNEL_URL>>`
    - `Server side rendering engine configuration URL` - `https://<<TUNNEL_URL>>/api/editing/config`
6. Click the Home icon in the top left corner of the Content Editor (the nine square grid icon).
7. Click on the Pages icon
8. You will be taken to your Pages instance, which is now connected to the head application running on your local devleoper machine. You can now add and remove components from the page and see the changes reflected in real-time. Please note the known issues stated above to see which components are not yet supported.