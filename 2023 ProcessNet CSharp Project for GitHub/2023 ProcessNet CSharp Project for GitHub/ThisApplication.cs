#region namespace
using System;
using System.Drawing;
using Microsoft.VisualBasic;
using System.Windows.Forms; //IWin32Window
using System.IO;
using System.Collections; //ArrayList
using System.Collections.Generic; //List<>

using FunctionBay.RecurDyn.ProcessNet;
using RDPost = FunctionBay.Post.ProcessNet; // RecurDyn Post

//For C#
//using FunctionBay.RecurDyn.ProcessNet.Chart;
//using FunctionBay.RecurDyn.ProcessNet.MTT2D;
//using FunctionBay.RecurDyn.ProcessNet.FFlex;
//using FunctionBay.RecurDyn.ProcessNet.RFlex;
//using FunctionBay.RecurDyn.ProcessNet.Tire;


//For VB.NET
//Imports FunctionBay.RecurDyn.ProcessNet
//Imports FunctionBay.RecurDyn.ProcessNet.Chart
//Imports FunctionBay.RecurDyn.ProcessNet.MTT2D
//Imports FunctionBay.RecurDyn.ProcessNet.FFlex
//Imports FunctionBay.RecurDyn.ProcessNet.RFlex
#endregion

namespace _2023_ProcessNet_CSharp_Project_for_GitHub
{
    public class ThisApplication
    {
        public void HelloProcessNet()
        {
            application.PrintMessage("Hello ProcessNet");
            application.PrintMessage(application.ProcessNetVersion);
        }

        public void CreateBodyExample()
        {
            refFrame1 = modelDocument.CreateReferenceFrame();
            refFrame1.SetOrigin(100, 0, 0);

            refFrame2 = modelDocument.CreateReferenceFrame();
            refFrame2.SetOrigin(0, 200, 0);

            IReferenceFrame refFrame3 = modelDocument.CreateReferenceFrame();
            refFrame3.SetOrigin(100, 200, 0);

            IBody body1 = model.CreateBodyBox("body1", refFrame1, 150, 100, 100);
            application.PrintMessage(body1.Name);

            IBody body2 = model.CreateBodySphere("body2", refFrame2, 50);
            application.PrintMessage(body2.Name);

            IBody body3 = model.CreateBodyCylinder("nody3", refFrame3, 100, 100);

        }

        #region Common Variables
        FunctionBay.RecurDyn.ProcessNet.RecurDyn.IRecurDynApp app = new FunctionBay.RecurDyn.ProcessNet.RecurDyn.RDApplication();
        static public IApplication application;
        public IModelDocument modelDocument = null;
        public IPlotDocument plotDocument = null;
        public ISubSystem model = null;

        public IReferenceFrame refFrame1 = null;
        public IReferenceFrame refFrame2 = null;

        public static RDPost.IPostMainWindow PostMainWindow { get; set; }
        public static RDPost.IMainDocument MainDocument { get; set; }
        #endregion

        #region Initialize and Dispose By RecurDyn
        public void Initialize()
        {
            application = app.RecurDynApplication as IApplication;
            modelDocument = application.ActiveModelDocument;
            plotDocument = application.ActivePlotDocument;

            if (modelDocument == null & plotDocument == null)
            {
                application.PrintMessage("No model file");
                modelDocument = application.NewModelDocument("Examples");
            }

            if (modelDocument != null)
            {
                model = modelDocument.Model;
            }
        }

        public void InitializeAndRunRecurDynPost()
        {
            var postApp = new RDPost.PostApplication();
            PostMainWindow = postApp.PostMainWindow as RDPost.IPostMainWindow;
            MainDocument = PostMainWindow.MainDocument;
        }
        public void Dispose()
        {
            modelDocument = application.ActiveModelDocument;
            if (modelDocument != null)
            {
                if (modelDocument.Validate() == true)
                {
                    //Redraw() and UpdateDatabaseWindow() can take more time in a heavy model.
                    modelDocument.Redraw();
                    //modelDocument.PostProcess(); //UpdateDatabaseWindow(), SetModified();
                    modelDocument.UpdateDatabaseWindow(); //If you call SetModified(), Animation will be reset.
                    modelDocument.SetModified();
                    modelDocument.SetUndoHistory("ProcessNet");
                }
            }
            if (MainDocument != null)
            {
                MainDocument.UpdateView();
            }
        }
        #endregion

        #region WinForms
        //private System.Windows.Forms.IWin32Window MainWindow;

        //If you made your own Form, please Show with argurment, MainWindow
        //MyForm.Show(MainWindow);

        class WinWrapper : System.Windows.Forms.IWin32Window
        {
            public WinWrapper(IntPtr oHandle)
            {
                _oHwnd = oHandle;
            }

            public IntPtr Handle
            {
                get { return _oHwnd; }
            }

            private IntPtr _oHwnd;
        }
        #endregion
    }
}
