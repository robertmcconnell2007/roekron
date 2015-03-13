# Introduction #

Before you can begin working on the tile engine and Roekron, you must first create a project in which to place these files. Since C# compilers must create computer specific data for files being compiled, we can only move around .cs files and resource files such as texture and .xml files. Please contact Lorin(ignatusfordon@gmail.com) if you have any issues getting the tile engine working on your computer.

# Details #

To begin, create a new project. This new project will be a **Windows Game Project** and will be named tileEngine.

Once this project is created, you must create a new library for this project. To do so, right click on the "Solution 'tileEngine'" and select add, new project. This new project will be a **Windows Game Library** and will be named tileEngineData. Delete the file named class1.cs as we will not be using it. Also, right click tileEngineData, add reference, click .net and select System.Xml

Next, right click the "Solution 'tileEngine'" once again and select add, new project. This last new project will be a **Content Pipeline Exension Library** and will be named tileEnginePipeline. Delete the file named ContentProcessor1.cs as we will not be using it.

Now we need to make sure these projects will reference eachother.
First right click on the tileEngine node and select add reference. Click projects, tileEngineData.
Then right click the content node within the tileEngine project and select add reference, projects, tileEnginePipeline.
Now right click the tileEnginePipeline node and select add reference. Click projects, tileEngineData

Now that the projects are created, we must add in the files you recieved from this site. For windows vista users, simply copy the tileEngine file from your tortise folder and merge it with the file in visual studio 2008 projects folder. Other users will have to manually copy over the files to the proper locations. Now that the files are all in place, you must add each to the proper location. please note that the content folders may not properly merge with the Content folder hierarchy within C# so you will have to move the Maps and Textures folders to make sure they are in the correct Content folder. The correct content folder contains a folder called References, so Maps and Textures should follow. In addition, this should only be the case for the tileEditor project, the other projects should end up in the proper locations.

An easy way to get these files into the project, is to select the "Show All Files" option in the solution explorer. This shows files that are not in the project. **Please be advised**, do not add all files seen with this option on as it could cause errors during build. Do not add bin obj or any other files that were not copied over from the tortise folder.