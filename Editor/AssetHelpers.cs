﻿using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace OmiyaGames.Common.Editor
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="AssetHelpers.cs" company="Omiya Games">
    /// The MIT License (MIT)
    /// 
    /// Copyright (c) 2014-2020 Omiya Games
    /// 
    /// Permission is hereby granted, free of charge, to any person obtaining a copy
    /// of this software and associated documentation files (the "Software"), to deal
    /// in the Software without restriction, including without limitation the rights
    /// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    /// copies of the Software, and to permit persons to whom the Software is
    /// furnished to do so, subject to the following conditions:
    /// 
    /// The above copyright notice and this permission notice shall be included in
    /// all copies or substantial portions of the Software.
    /// 
    /// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    /// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    /// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    /// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    /// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    /// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    /// THE SOFTWARE.
    /// </copyright>
    /// <list type="table">
    /// <listheader>
    /// <term>Revision</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>
    /// <strong>Date:</strong> 8/18/2015<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>
    /// Initial version.
    /// </description>
    /// </item>
    /// <item>
    /// <term>
    /// <strong>Version:</strong> 0.1.0-preview.1<br/>
    /// <strong>Date:</strong> 3/25/2020<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>
    /// Converted the class to a package.
    /// </description>
    /// </item>
    /// <item>
    /// <term>
    /// <strong>Version:</strong> 0.1.4-preview.1<br/>
    /// <strong>Date:</strong> 5/27/2020<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>
    /// Updating documentation to be compatible with DocFX.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// A series of utilities used throughout the <see cref="OmiyaGames.Common.Editor"/> namespace.
    /// This library focuses on assets-related static functions.
    /// </summary>
    public static class AssetHelpers
    {
        /// <summary>
        /// Default folder relative to Project root to create assets in.
        /// </summary>
        public const string CreateScriptableObjectAtFolder = "Assets/";
        /// <summary>
        /// File extension for manifest files.
        /// </summary>
        public const string ManifestFileExtension = ".manifest";
        /// <summary>
        /// Dialog title for overwriting files.
        /// </summary>
        public const string ConfirmationDialogTitle = "Overwrite File?";

        /// <summary>
        /// From an absolute path, grab the child-most folder name.
        /// </summary>
        /// <param name="path">The absolute path.</param>
        /// <param name="pathIncludesFileName">
        /// Flag indicating if <paramref name="path"/> contains a file name or not.
        /// </param>
        /// <returns>The child-most folder name.</returns>
        public static string GetLastFolderName(string path, bool pathIncludesFileName)
        {
            string returnPath = Path.GetFileName(path);
            if ((pathIncludesFileName == true) || (string.IsNullOrEmpty(returnPath) == true))
            {
                returnPath = Path.GetFileName(Path.GetDirectoryName(path));
            }
            return returnPath;
        }

        /// <summary>
        /// Starting with the child-most folder that does exist in
        /// <paramref name="newFolderPath"/>, start creating folders
        /// until <paramref name="newFolderPath"/> becomes an existing
        /// absolute path.
        /// <seealso cref="AssetDatabase.CreateFolder(string, string)"/>.
        /// </summary>
        /// <param name="newFolderPath">Absolute path to a non-existant folder.</param>
        /// <returns>
        /// <see cref="GUID"/> returned by the final call of
        /// <see cref="AssetDatabase.CreateFolder(string, string)"/>.
        /// </returns>
        public static string CreateFolderRecursively(string newFolderPath)
        {
            // Setup return value
            string returnGuid = null;

            // Check to see if the provided path ends properly
            string lastFolder = Path.GetFileName(newFolderPath);
            if (string.IsNullOrEmpty(lastFolder) == true)
            {
                newFolderPath = Path.GetDirectoryName(newFolderPath);
            }
            if (AssetDatabase.IsValidFolder(newFolderPath) == false)
            {
                // Create a stack of folders that doesn't exist yet
                Stack<string> newFolders = new Stack<string>();
                do
                {
                    // Push the last folder into the stack
                    lastFolder = GetLastFolderName(newFolderPath, false);
                    if (string.IsNullOrEmpty(lastFolder) == false)
                    {
                        newFolders.Push(lastFolder);
                    }

                    // Reduce the newFolderPath by the path
                    newFolderPath = Path.GetDirectoryName(newFolderPath);
                } while (AssetDatabase.IsValidFolder(newFolderPath) == false);

                // Go through the stack of new folders to create
                while (newFolders.Count > 0)
                {
                    lastFolder = newFolders.Pop();
                    returnGuid = AssetDatabase.CreateFolder(newFolderPath, lastFolder);
                    newFolderPath = Path.Combine(newFolderPath, lastFolder);
                }
            }
            return returnGuid;
        }

        /// <summary>
        /// Retrieves the folder from the selected asset in the Project window.
        /// </summary>
        /// <returns>Folder name of the selected asset.</returns>
        public static string GetSelectedFolder()
        {
            string returnPath = null;

            // Check if there's a selected object
            var obj = Selection.activeObject;
            if (obj != null)
            {
                returnPath = AssetDatabase.GetAssetPath(obj.GetInstanceID());
                if ((string.IsNullOrEmpty(returnPath) == false) && (Directory.Exists(returnPath) == false))
                {
                    returnPath = Path.GetDirectoryName(returnPath);
                }
            }

            if (string.IsNullOrEmpty(returnPath) == true)
            {
                returnPath = CreateScriptableObjectAtFolder;
            }
            return returnPath;
        }

        /// <summary>
        /// Checks if a file exists, and if so, optionally prompts the user
        /// if they want to overwrite it.
        /// </summary>
        /// <param name="pathOfAsset">
        /// Absolute path of the asset to write to.
        /// </param>
        /// <param name="nameOfFile">
        /// The name of the file, used to display the file that's going to be overwritten.
        /// </param>
        /// <param name="showWindow">
        /// If true, show a pop-up window prompting the user if they want to overwrite the file.
        /// </param>
        /// <returns>
        /// True if either file doesn't exist, or user confirms to overwrite it.
        /// </returns>
        public static bool ConfirmOverwriteFile(string pathOfAsset, string nameOfFile, bool showWindow = true)
        {
            // Check to see if file exists
            bool isBuildConfirmed = (File.Exists(pathOfAsset) == false);
            if ((isBuildConfirmed == false) && (showWindow == true))
            {
                // Create a message to indicate to the user
                StringBuilder builder = new StringBuilder();
                builder.Append("File \"");
                builder.Append(nameOfFile);
                builder.Append("\" already exists. Are you sure you want to overwrite this file?");

                // Bring up a pop-up confirming the file will be overwritten
                isBuildConfirmed = EditorUtility.DisplayDialog(ConfirmationDialogTitle, builder.ToString(), "Yes", "No");
            }
            return isBuildConfirmed;
        }

        /// <summary>
        /// Saves an asset bundle.
        /// </summary>
        /// <param name="newAsset">The single asset to bundle.</param>
        /// <param name="newFolderName">
        /// Path to the folder to create the asset bundle to.
        /// If it doesn't exist yet, that folder will be created.
        /// </param>
        /// <param name="newFileName">Name of the asset bundle file.</param>
        /// <param name="bundleId">
        /// Sets <see cref="AssetImporter.assetBundleName"/>.
        /// </param>
        /// <param name="builder">
        /// <see cref="StringBuilder"/> to create full path, etc.
        /// </param>
        /// <param name="relativeToProject">
        /// Flag indicating if <paramref name="newFolderName"/> is relative to
        /// the root of the project, or absolute path.
        /// </param>
        /// <param name="overwritePreviousFile">
        /// If set to true, automatically overwrites the asset bundle that already exists.
        /// </param>
        /// <returns>Path of the new asset.</returns>
        public static string SaveAsAssetBundle(ScriptableObject newAsset, string newFolderName, string newFileName, string bundleId, StringBuilder builder, bool relativeToProject, bool overwritePreviousFile = false)
        {
            // Generate the asset bundle at the Assets folder
            string pathOfAsset = GenerateScriptableObject(newAsset, newFileName, bundleId, builder);
            GenerateAssetBundle(bundleId, pathOfAsset);

            // Clean-up the rest of the assets
            CleanUpFiles(builder, pathOfAsset, bundleId);

            if (string.IsNullOrEmpty(newFolderName) == false)
            {
                if (relativeToProject == true)
                {
                    // Create a new folder if one doesn't exist
                    CreateFolderRecursively(newFolderName);
                }
                else if (Directory.Exists(newFolderName) == false)
                {
                    // Create a new folder if one doesn't exist
                    Directory.CreateDirectory(newFolderName);
                }

                // Move the created asset bundle to the designated location
                MoveAssetBundleTo(builder, newFolderName, newFileName, bundleId, relativeToProject, overwritePreviousFile);
            }
            return pathOfAsset;
        }

        #region SaveAsAssetBundle helpers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newAsset"></param>
        /// <param name="fileName"></param>
        /// <param name="bundleId"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        private static string GenerateScriptableObject(ScriptableObject newAsset, string fileName, string bundleId, StringBuilder builder)
        {
            // Generate a path to create an AcceptedDomainList
            builder.Clear();
            builder.Append(Path.Combine(CreateScriptableObjectAtFolder, fileName));
            builder.Append(Helpers.FileExtensionScriptableObject);
            string pathOfAsset = AssetDatabase.GenerateUniqueAssetPath(builder.ToString());

            // Create the AcceptedDomainList at the assigned path
            AssetDatabase.CreateAsset(newAsset, pathOfAsset);

            // Set its asset bundle name
            if (string.IsNullOrEmpty(bundleId) == false)
            {
                AssetImporter.GetAtPath(pathOfAsset).assetBundleName = bundleId;
            }

            // Save and refresh this asset
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return pathOfAsset;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bundleId"></param>
        /// <param name="objectPaths"></param>
        private static void GenerateAssetBundle(string bundleId, params string[] objectPaths)
        {
            // Create the array of bundle build details.
            AssetBundleBuild[] buildMap = new AssetBundleBuild[1];

            buildMap[0].assetBundleName = bundleId;
            buildMap[0].assetNames = objectPaths;

            // Put the bundles in folderName
            BuildPipeline.BuildAssetBundles(CreateScriptableObjectAtFolder, buildMap, BuildAssetBundleOptions.None, BuildTarget.WebGL);

            // Save and refresh this asset
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="newFolderName"></param>
        /// <param name="newFileName"></param>
        /// <param name="bundleId"></param>
        /// <param name="relativeToProject"></param>
        /// <param name="overwritePreviousFile"></param>
        private static void MoveAssetBundleTo(StringBuilder builder, string newFolderName, string newFileName, string bundleId, bool relativeToProject, bool overwritePreviousFile)
        {
            // Generate paths for the old file, to move to the new one
            string newPath = Path.Combine(newFolderName, newFileName);
            string pathOfAsset = Path.Combine(CreateScriptableObjectAtFolder, bundleId);

            // Move the asset to the folder designated by the user
            if (relativeToProject == true)
            {
                FileUtil.ReplaceFile(pathOfAsset, newPath);
            }
            else
            {
                bool canMoveFile = true;
                if (File.Exists(newPath) == true)
                {
                    if (overwritePreviousFile == true)
                    {
                        File.Delete(newPath);
                    }
                    else if (EditorUtility.DisplayDialog("File already exists", "File already exists at '" + newPath + "'. Do you want to overwrite it?", "Yes", "No") == true)
                    {
                        File.Delete(newPath);
                    }
                    else
                    {
                        canMoveFile = false;
                    }
                }
                if (canMoveFile == true)
                {
                    File.Move(Path.Combine(Application.dataPath, bundleId), newPath);
                }
            }

            // Delete the old assets
            FileUtil.DeleteFileOrDirectory(pathOfAsset);

            if (relativeToProject == true)
            {
                // Refresh the project window
                AssetDatabase.Refresh();
                UnityEditor.EditorUtility.FocusProjectWindow();
                Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(newPath);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="acceptedDomainListObjectPath"></param>
        /// <param name="bundleId"></param>
        private static void CleanUpFiles(StringBuilder builder, string acceptedDomainListObjectPath, string bundleId)
        {
            // Clean-up the acceptedDomainListObject
            FileUtil.DeleteFileOrDirectory(acceptedDomainListObjectPath);

            // Clean-up the asset bundle for this folder
            string fileName = Path.GetFileName(Path.GetDirectoryName(CreateScriptableObjectAtFolder));
            builder.Clear();
            builder.Append(Path.Combine(CreateScriptableObjectAtFolder, fileName));
            FileUtil.DeleteFileOrDirectory(builder.ToString());

            // Clean-up the manifest files for this folder
            builder.Append(ManifestFileExtension);
            FileUtil.DeleteFileOrDirectory(builder.ToString());

            // Clean-up the manifest files for this folder
            builder.Clear();
            builder.Append(Path.Combine(CreateScriptableObjectAtFolder, bundleId));
            builder.Append(ManifestFileExtension);
            FileUtil.DeleteFileOrDirectory(builder.ToString());

            // Clean-up unused bundle IDs
            AssetDatabase.RemoveAssetBundleName(bundleId, false);
        }
        #endregion
    }
}
