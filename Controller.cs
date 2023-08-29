using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Security.Permissions;

namespace WF_HW_Search_Folders_and_Files
{
    internal class Controller
    {

        public List<FileInfo> list;

        public List<FileInfo> listByWors;
        
        
        public bool FlagThread = true;
        public bool FlagHaveCollect = false;
        public bool Flagsubdirectories = true;
     

      
        
        public void foo(object obj)
        {
            string[] str = obj as string[];
            string FileName = str[0];
            string WirdInFile = str[1];
            string Path = str[2];

            list = new List<FileInfo>();
            listByWors = new List<FileInfo>();
            
            try
            {
                foreach (var el in Directory.GetFiles(Path, FileName, SearchOption.TopDirectoryOnly))   
                {
                    FileInfo FI = new FileInfo(el);
                    list.Add(FI);
                }

                if (Flagsubdirectories)  //если указаны подкаталоги ищем в корне
                {
                    foreach (var el in Directory.GetDirectories(Path))      //берем папки корня
                    {
                        try { 
                        DirectoryInfo DI = new DirectoryInfo(el);

                            foreach (var el2 in Directory.GetFiles(el, FileName, SearchOption.AllDirectories))
                            {
                                //Directory.Exist();
                                DirectoryInfo DI2 = new DirectoryInfo(el2);

                                if (DI2.Attributes.HasFlag(FileAttributes.System) || DI2.Attributes.HasFlag(FileAttributes.Hidden)) continue;

                                FileInfo FIn = new FileInfo(el2);
                                list.Add(FIn);
                            }
                        }
                        catch { }

                    }
                }

            }

            catch (Exception ex) { MessageBox.Show(ex.Message); }
            
        }



        public void FindFile(object obj)
        {
            FlagThread = true;
            string[] str = obj as string[];

            string FileName = str[0];
            string WordInFile = str[1];
            string Path = str[2];

            foo(obj);
            if (WordInFile != null)
            {
                foreach (var el in list)      //берем папки корня
                {
                    try
                    {


                        FileInfo FIn = new FileInfo(el.FullName);

                        StreamReader streamReader = new StreamReader(FIn.FullName);
                        string satr = streamReader.ReadToEnd();
                        if (satr.Contains(WordInFile))
                        {
                            listByWors.Add(FIn);
                        }


                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }

                }
            }

            FlagThread = false ;

        }







    }
}
