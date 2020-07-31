using PU.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Windows.Forms;

namespace PU
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            string path86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            OperatingSystem OS = Environment.OSVersion;



            if ((Application.StartupPath.Contains(path) || Application.StartupPath.Contains(path86)) && OS.Version.Major > 5)  // если программа запускается из системного каталога Program Files и версия больше Windows XP
            {
                WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                bool hasAdministrativeRight = pricipal.IsInRole(WindowsBuiltInRole.Administrator);

                if (hasAdministrativeRight == false)
                {
                    ProcessStartInfo processInfo = new ProcessStartInfo(); //создаем новый процесс
                    processInfo.Verb = "runas"; //в данном случае указываем, что процесс должен быть запущен с правами администратора
                    processInfo.FileName = Application.ExecutablePath; //указываем исполняемый файл (программу) для запуска
                    try
                    {
                        Process.Start(processInfo); //пытаемся запустить процесс
                    }
                    catch (Win32Exception)
                    {
                        //Ничего не делаем, потому что пользователь, возможно, нажал кнопку "Нет" в ответ на вопрос о запуске программы в окне предупреждения UAC (для Windows 7)
                    }
                    Application.Exit(); //закрываем текущую копию программы (в любом случае, даже если пользователь отменил запуск с правами администратора в окне UAC)
                }
                else //имеем права администратора, значит, стартуем
                {

                    //SetProcessDPIAware();

                    if (args.Count() > 0)
                        readArgs(args);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainForm());
                }
            }
            else
            {
                //SetProcessDPIAware();
                if (args.Count() > 0)
                    readArgs(args);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }



        }

        //[System.Runtime.InteropServices.DllImport("user32.dll")]
        //private static extern bool SetProcessDPIAware();

        static void readArgs(string[] args)
        {
            try
            {
                Options.settingsFilePath = !String.IsNullOrEmpty(args[0]) ? args[0] : Application.StartupPath + "\\settings.xml";
                var configPath = Path.GetDirectoryName(Options.settingsFilePath) + "\\PU.exe.Config";
                if (!File.Exists(configPath))
                {
                    System.IO.File.Copy(Application.StartupPath + "\\PU.exe.Config", configPath);
                }
                AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", configPath);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
