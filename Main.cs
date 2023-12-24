using Life;
using Life.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using MyMenu.Entities;
using System.IO;
using Newtonsoft.Json;

namespace MyPresto
{
    public class Main : Plugin
    {
        public static string DirectoryPath;
        public static string Filename = "BlockedItems.json";
        private readonly EventsTriggers Events;
        public static List<int> BlockedItems = new List<int>();

        public Main(IGameAPI api): base(api)
        {
            Events = new EventsTriggers();

            //MyMenu
            try
            {
                Section section = new Section(Section.GetSourceName(), Section.GetSourceName(), "v1.0.0", "Aarnow");
                Action<UIPanel> action = ui => AdminPanels.Open(section.GetPlayer(ui));
                section.OnlyAdmin = true;
                section.Line = new UITabLine(section.Title, action);
                section.Insert(true);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            Debug.Log($"Plugin \"MyPresto\" initialisé avec succès.");
        }

        public override void OnPluginInit()
        {
            base.OnPluginInit();
            InitDirectory();
            LoadBlockedItems();
            Events.Init(Nova.server);
        }

        public void InitDirectory()
        {
            DirectoryPath = pluginsPath + "/MyPresto";
            if (!Directory.Exists(DirectoryPath)) Directory.CreateDirectory(DirectoryPath);
        }

        public void LoadBlockedItems()
        {        
            string filePath = Path.Combine(DirectoryPath, Filename);
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                BlockedItems = JsonConvert.DeserializeObject<List<int>>(json);
            }
            else Utils.SaveBlockedItem();
        }
    }
}