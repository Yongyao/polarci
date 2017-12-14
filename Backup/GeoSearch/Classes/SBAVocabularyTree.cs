using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using GeoSearch.CSWQueryServiceReference;

namespace GeoSearch
{
    public class SBAVocabularyTree : INotifyPropertyChanged
    {
        public const string SBA_Disasters = "Disasters";
        public const string SBA_Disasters_PollutionEvents = "Pollution Events";
        public const string SBA_Disasters_CoastalHazards  = "Coastal Hazards";
        public const string SBA_Disasters_SeaAndLakeIce = "Sea and Lake Ice";
        public const string SBA_Disasters_TropicalCyclones = "Tropical Cyclones";
        public const string SBA_Disasters_ExtremeWeather = "Extreme Weather";
        public const string SBA_Disasters_Flood = "Flood";
        public const string SBA_Disasters_Landslides = "Landslides";
        public const string SBA_Disasters_Volcanoes = "Volcanoes";
        public const string SBA_Disasters_Earthquakes = "Earthquakes";
        public const string SBA_Disasters_WildlandFires = "Wildland Fires";

        public const string SBA_Health = "Health";
        public const string SBA_Health_InfectiousDiseases = "Infectious Diseases";
        public const string SBA_Health_Cancer = "Cancer";
        public const string SBA_Health_RespiratoryProblems = "Respiratory Problems";
        public const string SBA_Health_EnvironmentalStress = "Environmental Stress";
        public const string SBA_Health_Nutrition = "Nutrition";
        public const string SBA_Health_Accidentals = "Accidentals";
        public const string SBA_Health_BirthDefect = "Birth Defect";

        public const string SBA_Energy = "Energy";
        public const string SBA_Energy_OilGas  = "Oil & Gas";
        public const string SBA_Energy_RefiningTransport = "Refining & Transport";
        public const string SBA_Energy_RenewableEnergy = "Renewable Energy";
        public const string SBA_Energy_ElectricityGeneration = "Electricity Generation";
        public const string SBA_Energy_GlobalEnergy = "Global Energy";

        public const string SBA_Climate = "Climate";
        public const string SBA_Climate_Understanding = "Understanding";
        public const string SBA_Climate_Assessing = "Assessing";
        public const string SBA_Climate_Predicting = "Predicting";
        public const string SBA_Climate_AdaptingTo = "Adapting to";
        public const string SBA_Climate_Mitigating = "Mitigating";

        public const string SBA_Water = "Water";
        public const string SBA_Water_WaterCycle = "Water Cycle";
        public const string SBA_Water_ResourceManagement = "Resource Management";
        public const string SBA_Water_ImpactsOfHumans = "Impacts of Humans";
        public const string SBA_Water_Biogeochemistry = "Biogeochemistry";
        public const string SBA_Water_Ecosystem = "Ecosystem";
        public const string SBA_Water_LandUsePlanning = "Land Use Planning";
        public const string SBA_Water_ProductionOfFood = "Production of Food";
        public const string SBA_Water_WeatherPrediction = "Weather Prediction";
        public const string SBA_Water_FloodPrediction = "Flood Prediction";
        public const string SBA_Water_DroughtPrediction = "Drought Prediction";
        public const string SBA_Water_ClimatePrediction = "Climate Prediction";
        public const string SBA_Water_HumanHealth = "Human Health";
        public const string SBA_Water_FisheriesAndHabitat = "Fisheries and Habitat";
        public const string SBA_Water_Management = "Management";
        public const string SBA_Water_TelecomunicationNavigation  = "Telecomunication Navigation";
   
        public const string SBA_Weather = "Weather";
        public const string SBA_Weather_Nowcasting0_2hs = "Nowcasting 0 - 2 hs";
        public const string SBA_Weather_ShortRange2_72hs  = "Short Range 2 - 72 hs";
        public const string SBA_Weather_MediumRange3_10days  = "Medium Range 3 - 10 days";
        public const string SBA_Weather_Extended10_30days  = "Extended 10 - 30 days";

        public const string SBA_Ecosystems = "Ecosystems";
        public const string SBA_Ecosystems_LandRiverCoastOcean  = "Land, River, Coast & Ocean";
        public const string SBA_Ecosystems_AgricultureFisheriesForestry = "Agriculture, Fisheries, Forestry";
        public const string SBA_Ecosystems_CarbonCycle = "Carbon Cycle";

        public const string SBA_Agriculture = "Agriculture";
        public const string SBA_Agriculture_FoodSecurity = "Food Security";
        public const string SBA_Agriculture_Fisheries = "Fisheries";
        public const string SBA_Agriculture_TimberFuelFiber = "Timber, Fuel & Fiber";
        public const string SBA_Agriculture_EconomyTrade = "Economy & Trade";
        public const string SBA_Agriculture_GrazingSystems = "Grazing Systems";

        public const string SBA_Biodiversity = "Biodiversity";
        public const string SBA_Biodiversity_Conservation = "Conservation";
        public const string SBA_Biodiversity_InvasiveSpecies = "Invasive Species";
        public const string SBA_Biodiversity_MigratorySpecies = "Migratory Species";
        public const string SBA_Biodiversity_NaturalResources = "Natural Resources";

        public string Name { get; set; }
        public string PictureURL { get; set; }
        public ObservableCollection<SBAVocabularyTree> Children { get; set; }
        public SBAVocabularyTree Parent { get; set; }
        public string SBAVocabularyID { get; set; }
        public string Description { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isSelectedValue = false;
        public bool isSelected
        {
            get
            {
                return this.isSelectedValue;
            }

            set
            {
                if (value != this.isSelectedValue)
                {
                    this.isSelectedValue = value;
                    NotifyPropertyChanged("isSelected");
                }
            }
        }

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public SBAVocabularyTree(string name, string description, string pictureUrl, bool select, string id, ObservableCollection<SBAVocabularyTree> children)
        {
            Name = name;
            PictureURL = pictureUrl;
            Children = children;
            isSelected = select;
            SBAVocabularyID = id;
            Description = description;
            if (Children != null)
                foreach (SBAVocabularyTree r in Children)
                {
                    r.Parent = this;
                }
        }

        public static ObservableCollection<SBAVocabularyTree> getSBAVocabularyList()
        {
            SBAVocabularyTree PollutionEvents = new SBAVocabularyTree(SBA_Disasters_PollutionEvents, SBA_Disasters_PollutionEvents, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Disasters_PollutionEvents, null);
            SBAVocabularyTree CoastalHazards = new SBAVocabularyTree(SBA_Disasters_CoastalHazards, SBA_Disasters_CoastalHazards, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Disasters_CoastalHazards, null);
            SBAVocabularyTree SeaAndLakeIce = new SBAVocabularyTree(SBA_Disasters_SeaAndLakeIce, SBA_Disasters_SeaAndLakeIce, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Disasters_SeaAndLakeIce, null);
            SBAVocabularyTree TropicalCyclones = new SBAVocabularyTree(SBA_Disasters_TropicalCyclones, SBA_Disasters_TropicalCyclones, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Disasters_TropicalCyclones, null);
            SBAVocabularyTree ExtremeWeather = new SBAVocabularyTree(SBA_Disasters_ExtremeWeather, SBA_Disasters_ExtremeWeather, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Disasters_ExtremeWeather, null);
            SBAVocabularyTree Flood = new SBAVocabularyTree(SBA_Disasters_Flood, SBA_Disasters_Flood, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Disasters_Flood, null);
            SBAVocabularyTree Landslides = new SBAVocabularyTree(SBA_Disasters_Landslides, SBA_Disasters_Landslides, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Disasters_Landslides, null);
            SBAVocabularyTree Volcanoes = new SBAVocabularyTree(SBA_Disasters_Volcanoes, SBA_Disasters_Volcanoes, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Disasters_Volcanoes, null);
            SBAVocabularyTree Earthquakes = new SBAVocabularyTree(SBA_Disasters_Earthquakes, SBA_Disasters_Earthquakes, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Disasters_Earthquakes, null);
            SBAVocabularyTree WildlandFires = new SBAVocabularyTree(SBA_Disasters_WildlandFires, SBA_Disasters_WildlandFires, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Disasters_WildlandFires, null);
            ObservableCollection<SBAVocabularyTree> DisastersList = new ObservableCollection<SBAVocabularyTree> { PollutionEvents, CoastalHazards, SeaAndLakeIce, TropicalCyclones, ExtremeWeather, Flood, Landslides, Volcanoes, Earthquakes, WildlandFires};
            SBAVocabularyTree Disasters = new SBAVocabularyTree(SBA_Disasters, SBA_Disasters, "/GeoSearch;component/images/resourceTypes/map.png", true, SBA_Disasters, DisastersList);

            SBAVocabularyTree InfectiousDiseases = new SBAVocabularyTree(SBA_Health_InfectiousDiseases, SBA_Health_InfectiousDiseases, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Health_InfectiousDiseases, null);
            SBAVocabularyTree Cancer = new SBAVocabularyTree(SBA_Health_Cancer, SBA_Health_Cancer, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Health_Cancer, null);
            SBAVocabularyTree RespiratoryProblems = new SBAVocabularyTree(SBA_Health_RespiratoryProblems, SBA_Health_RespiratoryProblems, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Health_RespiratoryProblems, null);
            SBAVocabularyTree EnvironmentalStress = new SBAVocabularyTree(SBA_Health_EnvironmentalStress, SBA_Health_EnvironmentalStress, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Health_EnvironmentalStress, null);
            SBAVocabularyTree Nutrition = new SBAVocabularyTree(SBA_Health_Nutrition, SBA_Health_Nutrition, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Health_Nutrition, null);
            SBAVocabularyTree Accidentals = new SBAVocabularyTree(SBA_Health_Accidentals, SBA_Health_Accidentals, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Health_Accidentals, null);
            SBAVocabularyTree BirthDefect = new SBAVocabularyTree(SBA_Health_BirthDefect, SBA_Health_BirthDefect, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Health_BirthDefect, null);
            ObservableCollection<SBAVocabularyTree> HealthList = new ObservableCollection<SBAVocabularyTree> { InfectiousDiseases, Cancer, RespiratoryProblems, EnvironmentalStress, Nutrition, Accidentals, BirthDefect};
            SBAVocabularyTree Health = new SBAVocabularyTree(SBA_Health, SBA_Health, "/GeoSearch;component/images/resourceTypes/map.png", true, SBA_Health, HealthList);

            SBAVocabularyTree OilGas = new SBAVocabularyTree(SBA_Energy_OilGas, SBA_Energy_OilGas, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Energy_OilGas, null);
            SBAVocabularyTree RefiningTransport = new SBAVocabularyTree(SBA_Energy_RefiningTransport, SBA_Energy_RefiningTransport, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Energy_RefiningTransport, null);
            SBAVocabularyTree RenewableEnergy = new SBAVocabularyTree(SBA_Energy_RenewableEnergy, SBA_Energy_RenewableEnergy, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Energy_RenewableEnergy, null);
            SBAVocabularyTree ElectricityGeneration = new SBAVocabularyTree(SBA_Energy_ElectricityGeneration, SBA_Energy_ElectricityGeneration, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Energy_ElectricityGeneration, null);
            SBAVocabularyTree GlobalEnergy = new SBAVocabularyTree(SBA_Energy_GlobalEnergy, SBA_Energy_GlobalEnergy, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Energy_GlobalEnergy, null);
            ObservableCollection<SBAVocabularyTree> EnergyList = new ObservableCollection<SBAVocabularyTree> { OilGas, RefiningTransport, RenewableEnergy, ElectricityGeneration, GlobalEnergy};
            SBAVocabularyTree Energy = new SBAVocabularyTree(SBA_Energy, SBA_Energy, "/GeoSearch;component/images/resourceTypes/map.png", true, SBA_Energy, EnergyList);

            SBAVocabularyTree Understanding = new SBAVocabularyTree(SBA_Climate_Understanding, SBA_Climate_Understanding, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Climate_Understanding, null);
            SBAVocabularyTree Assessing = new SBAVocabularyTree(SBA_Climate_Assessing, SBA_Climate_Assessing, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Climate_Assessing, null);
            SBAVocabularyTree Predicting = new SBAVocabularyTree(SBA_Climate_Predicting, SBA_Climate_Predicting, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Climate_Predicting, null);
            SBAVocabularyTree AdaptingTo = new SBAVocabularyTree(SBA_Climate_AdaptingTo, SBA_Climate_AdaptingTo, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Climate_AdaptingTo, null);
            SBAVocabularyTree Mitigating = new SBAVocabularyTree(SBA_Climate_Mitigating, SBA_Climate_Mitigating, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Climate_Mitigating, null);
            ObservableCollection<SBAVocabularyTree> ClimateList = new ObservableCollection<SBAVocabularyTree> { Understanding, Assessing, Predicting, AdaptingTo, Mitigating };
            SBAVocabularyTree Climate = new SBAVocabularyTree(SBA_Climate, SBA_Climate, "/GeoSearch;component/images/resourceTypes/map.png", true, SBA_Climate, ClimateList);

            SBAVocabularyTree WaterCycle = new SBAVocabularyTree(SBA_Water_WaterCycle, SBA_Water_WaterCycle, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Water_WaterCycle, null);
            SBAVocabularyTree ResourceManagement = new SBAVocabularyTree(SBA_Water_ResourceManagement, SBA_Water_ResourceManagement, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Water_ResourceManagement, null);
            SBAVocabularyTree ImpactsOfHumans = new SBAVocabularyTree(SBA_Water_ImpactsOfHumans, SBA_Water_ImpactsOfHumans, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Water_ImpactsOfHumans, null);
            SBAVocabularyTree Biogeochemistry = new SBAVocabularyTree(SBA_Water_Biogeochemistry, SBA_Water_Biogeochemistry, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Water_Biogeochemistry, null);
            SBAVocabularyTree Ecosystem = new SBAVocabularyTree(SBA_Water_Ecosystem, SBA_Water_Ecosystem, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Water_Ecosystem, null);
            SBAVocabularyTree LandUsePlanning = new SBAVocabularyTree(SBA_Water_LandUsePlanning, SBA_Water_LandUsePlanning, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Water_LandUsePlanning, null);
            SBAVocabularyTree ProductionOfFood = new SBAVocabularyTree(SBA_Water_ProductionOfFood, SBA_Water_ProductionOfFood, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Water_ProductionOfFood, null);
            SBAVocabularyTree WeatherPrediction = new SBAVocabularyTree(SBA_Water_WeatherPrediction, SBA_Water_WeatherPrediction, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Water_WeatherPrediction, null);
            SBAVocabularyTree FloodPrediction = new SBAVocabularyTree(SBA_Water_FloodPrediction, SBA_Water_FloodPrediction, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Water_FloodPrediction, null);
            SBAVocabularyTree DroughtPrediction = new SBAVocabularyTree(SBA_Water_DroughtPrediction, SBA_Water_DroughtPrediction, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Water_DroughtPrediction, null);
            SBAVocabularyTree ClimatePrediction = new SBAVocabularyTree(SBA_Water_ClimatePrediction, SBA_Water_ClimatePrediction, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Water_ClimatePrediction, null);
            SBAVocabularyTree HumanHealth = new SBAVocabularyTree(SBA_Water_HumanHealth, SBA_Water_HumanHealth, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Water_HumanHealth, null);
            SBAVocabularyTree FisheriesAndHabitat = new SBAVocabularyTree(SBA_Water_FisheriesAndHabitat, SBA_Water_FisheriesAndHabitat, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Water_FisheriesAndHabitat, null);
            SBAVocabularyTree Management = new SBAVocabularyTree(SBA_Water_Management, SBA_Water_Management, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Water_Management, null);
            SBAVocabularyTree TelecomunicationNavigation = new SBAVocabularyTree(SBA_Water_TelecomunicationNavigation, SBA_Water_TelecomunicationNavigation, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Water_TelecomunicationNavigation, null);
            ObservableCollection<SBAVocabularyTree> WaterList = new ObservableCollection<SBAVocabularyTree> { WaterCycle, ResourceManagement, ImpactsOfHumans, Biogeochemistry, Ecosystem, 
                LandUsePlanning, ProductionOfFood, ProductionOfFood, FloodPrediction, DroughtPrediction,
                ClimatePrediction, HumanHealth, FisheriesAndHabitat, Management, TelecomunicationNavigation};
            SBAVocabularyTree Water = new SBAVocabularyTree(SBA_Water, SBA_Water, "/GeoSearch;component/images/resourceTypes/map.png", true, SBA_Water, WaterList);

            SBAVocabularyTree Nowcasting0_2hs = new SBAVocabularyTree(SBA_Weather_Nowcasting0_2hs, SBA_Weather_Nowcasting0_2hs, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Weather_Nowcasting0_2hs, null);
            SBAVocabularyTree ShortRange2_72hs = new SBAVocabularyTree(SBA_Weather_ShortRange2_72hs, SBA_Weather_ShortRange2_72hs, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Weather_ShortRange2_72hs, null);
            SBAVocabularyTree MediumRange3_10days = new SBAVocabularyTree(SBA_Weather_MediumRange3_10days, SBA_Weather_MediumRange3_10days, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Weather_MediumRange3_10days, null);
            SBAVocabularyTree Extended10_30days = new SBAVocabularyTree(SBA_Weather_Extended10_30days, SBA_Weather_Extended10_30days, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Weather_Extended10_30days, null);
            ObservableCollection<SBAVocabularyTree> WeatherList = new ObservableCollection<SBAVocabularyTree> { Nowcasting0_2hs, ShortRange2_72hs, MediumRange3_10days, Extended10_30days };
            SBAVocabularyTree Weather = new SBAVocabularyTree(SBA_Weather, SBA_Weather, "/GeoSearch;component/images/resourceTypes/map.png", true, SBA_Weather, WeatherList);

            SBAVocabularyTree LandRiverCoastOcean = new SBAVocabularyTree(SBA_Ecosystems_LandRiverCoastOcean, SBA_Ecosystems_LandRiverCoastOcean, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Ecosystems_LandRiverCoastOcean, null);
            SBAVocabularyTree AgricultureFisheriesForestry = new SBAVocabularyTree(SBA_Ecosystems_AgricultureFisheriesForestry, SBA_Ecosystems_AgricultureFisheriesForestry, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Ecosystems_AgricultureFisheriesForestry, null);
            SBAVocabularyTree CarbonCycle = new SBAVocabularyTree(SBA_Ecosystems_CarbonCycle, SBA_Ecosystems_CarbonCycle, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Ecosystems_CarbonCycle, null);
            ObservableCollection<SBAVocabularyTree> EcosystemsList = new ObservableCollection<SBAVocabularyTree> { LandRiverCoastOcean, AgricultureFisheriesForestry, CarbonCycle};
            SBAVocabularyTree Ecosystems = new SBAVocabularyTree(SBA_Ecosystems, SBA_Ecosystems, "/GeoSearch;component/images/resourceTypes/map.png", true, SBA_Ecosystems, EcosystemsList);
            
            SBAVocabularyTree FoodSecurity = new SBAVocabularyTree(SBA_Agriculture_FoodSecurity, SBA_Agriculture_FoodSecurity, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Agriculture_FoodSecurity, null);
            SBAVocabularyTree Fisheries = new SBAVocabularyTree(SBA_Agriculture_Fisheries, SBA_Agriculture_Fisheries, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Agriculture_Fisheries, null);
            SBAVocabularyTree TimberFuelFiber = new SBAVocabularyTree(SBA_Agriculture_TimberFuelFiber, SBA_Agriculture_TimberFuelFiber, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Agriculture_TimberFuelFiber, null);
            SBAVocabularyTree EconomyTrade = new SBAVocabularyTree(SBA_Agriculture_EconomyTrade, SBA_Agriculture_EconomyTrade, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Agriculture_EconomyTrade, null);
            SBAVocabularyTree GrazingSystems = new SBAVocabularyTree(SBA_Agriculture_GrazingSystems, SBA_Agriculture_GrazingSystems, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Agriculture_GrazingSystems, null);
            ObservableCollection<SBAVocabularyTree> AgricultureList = new ObservableCollection<SBAVocabularyTree> { FoodSecurity, Fisheries, TimberFuelFiber, EconomyTrade, GrazingSystems };
            SBAVocabularyTree Agriculture = new SBAVocabularyTree(SBA_Agriculture, SBA_Agriculture, "/GeoSearch;component/images/resourceTypes/map.png", true, SBA_Agriculture, AgricultureList);
        
            SBAVocabularyTree Conservation = new SBAVocabularyTree(SBA_Biodiversity_Conservation, SBA_Biodiversity_Conservation, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Biodiversity_Conservation, null);
            SBAVocabularyTree InvasiveSpecies = new SBAVocabularyTree(SBA_Biodiversity_InvasiveSpecies, SBA_Biodiversity_InvasiveSpecies, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Biodiversity_InvasiveSpecies, null);
            SBAVocabularyTree MigratorySpecies = new SBAVocabularyTree(SBA_Biodiversity_MigratorySpecies, SBA_Biodiversity_MigratorySpecies, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Biodiversity_MigratorySpecies, null);
            SBAVocabularyTree NaturalResources = new SBAVocabularyTree(SBA_Biodiversity_NaturalResources, SBA_Biodiversity_NaturalResources, "/GeoSearch;component/images/SBAVocabulariess/map.png", true, SBA_Biodiversity_NaturalResources, null);
            ObservableCollection<SBAVocabularyTree> BiodiversityList = new ObservableCollection<SBAVocabularyTree> { Conservation, InvasiveSpecies, MigratorySpecies, NaturalResources};
            SBAVocabularyTree Biodiversity = new SBAVocabularyTree(SBA_Biodiversity, SBA_Biodiversity, "/GeoSearch;component/images/resourceTypes/map.png", true, SBA_Biodiversity, BiodiversityList);

            //ObservableCollection<SBAVocabularyTree> list = new ObservableCollection<SBAVocabularyTree> { Disasters, Health, Energy, Climate, Water, Weather, Ecosystems, Agriculture, Biodiversity };
            ObservableCollection<SBAVocabularyTree> list = new ObservableCollection<SBAVocabularyTree> { Agriculture, Biodiversity, Climate, Disasters, Ecosystems, Energy, Health, Water, Weather};
            return list;
        }

        public static SBAVocabulary createSBAVocabularyFromTreeNode(SBAVocabularyTree vt)
        {
            SBAVocabulary root = new SBAVocabulary();
            root.Name = vt.Name;
            root.isSelected = vt.isSelected;
            root.SBAVocabularyID = vt.SBAVocabularyID;
            if (vt.Children != null)
            {
                root.Children = new ObservableCollection<SBAVocabulary>();
                foreach (SBAVocabularyTree rtt in vt.Children)
                {
                    SBAVocabulary rt = createSBAVocabularyFromTreeNode(rtt);
                    root.Children.Add(rt);
                }
            }
            return root;
        }
    }
}
