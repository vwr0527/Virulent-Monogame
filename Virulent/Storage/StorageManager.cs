using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
//using Microsoft.Xna.Framework.Storage;using Microsoft.Xna.Framework;
using System.IO;

namespace Virulent.Storage
{
    class StorageManager
    {
//        StorageDevice m_device;
        IAsyncResult m_result;
        bool gameSaveRequested = false;

/*		public StorageManager()//StorageDevice deviceParam)
        {
            m_device = deviceParam;
        }
*/
        public StorageManager()
        {
//            m_device = null;
        }

        public void DoSaveRequest(bool guideIsVisible, PlayerIndex whichPlayer)
        {
            // Set the request flag
            if ((!guideIsVisible) && (gameSaveRequested == false))
            {
                gameSaveRequested = true;
                //m_result = StorageDevice.BeginShowSelector(whichPlayer, null, null);
            }
        }

        public void DoPendingSave()
        {
            if ((gameSaveRequested) && (m_result.IsCompleted))
            {
                /*m_device = StorageDevice.EndShowSelector(m_result);
                if (m_device != null && m_device.IsConnected)
                {
                    //this is where the saving actually happens.
                    MessWithFiles();
                }
                // Reset the request flag*/
                gameSaveRequested = false;
            }
        }

        /*private void MessWithFiles()
        {

            // Open a storage container.
            IAsyncResult result =
                m_device.BeginOpenContainer("StorageDemo", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = m_device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            // Add the container path to our file name.
            string filename = "demobinary.sav";

            // Create a new file.
            if (!container.FileExists(filename))
            {
                Stream file = container.CreateFile(filename);
                file.Close();
            }
            // Dispose the container, to commit the data.
            container.Dispose();
        }*/
    }
}
