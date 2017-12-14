using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;


// ?????

namespace GeoSearch.Web
{
    public class SBAVocabulary
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
        public ObservableCollection<SBAVocabulary> Children { get; set; }
        public SBAVocabulary Parent { get; set; }
        public string SBAVocabularyID { get; set; }
        public bool isSelected { get; set; }
    }
}
