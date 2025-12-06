using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class blueprintReader : MonoBehaviour
{
    //Google Drive Link:
    //https://drive.google.com/drive/u/0/folders/1p1RbBrjIHumLBaVQ3yRZapqv52vkomKT
    //Brick Colours:
    //https://create.roblox.com/docs/reference/engine/datatypes/BrickColor
    //Workshop Link:
    //https://www.roblox.com/games/4694452046/Workshop
    
    //Cube
    //Wedge
    //Cone
    //Vertical Cylinder
    //Horizontal Cylinder
    //Sphere
    //Cone

    public List<blueprintPart> blueprintList = new List<blueprintPart>();
    Dictionary<string, string> names = new Dictionary<string, string>()
    {
        {"AA", "Servo"},
        {"AB", "HeatSuit"},
        {"AC", "MechLimb"},
        {"AD", "Debris"},
        {"AE", "Stone"},
        {"AF", "Ice"},
        {"AG", "Lamp"},
        {"AH", "DoorTemplate"},
        {"AI", "Copper"},
        {"AJ", "Iron"},
        {"AK", "DriveBox"},
        {"AL", "Jade"},
        {"AM", "Transformer"},
        {"AN", "Arcolantium"},
        {"AO", "Brick"},
        {"AP", "Silicon"},
        {"AQ", "Gold"},
        {"AR", "Instrument"},
        {"AS", "Levitator"},
        {"AT", "HydroGenerator"},
        {"AU", "CarbonFiber"},
        {"AV", "Uranium"},
        {"AW", "Antenna"},
        {"AX", "Plastic"},
        {"AY", "Flint"},
        {"AZ", "Gear"},
        {"BA", "Diamond"},
        {"BB", "Eridanium"},
        {"BC", "Seat"},
        {"BD", "Ghostalum"},
        {"BE", "Sail"},
        {"BF", "Rubber"},
        {"BG", "Cleats"},
        {"BH", "SpotLight"},
        {"BI", "Turbofan"},
        {"BJ", "Cooler"},
        {"BK", "Anchor"},
        {"BL", "Debouncer"},
        {"BM", "Titanium"},
        {"BN", "CrudeWing"},
        {"BO", "Amethyst"},
        {"BP", "Glass"},
        {"BQ", "Wing"},
        {"BR", "Herbicide"},
        {"BS", "Leaves"},
        {"BT", "OrganicMatter"},
        {"BU", "Neodymium"},
        {"BV", "Thruster"},
        {"BW", "GravDevice"},
        {"BX", "SteeringSeat"},
        {"BY", "LightningRod"},
        {"BZ", "CMOS"},
        {"CA", "PsiButton"},
        {"CB", "Gyro"},
        {"CC", "Dynamite"},
        {"CD", "Refinery"},
        {"CE", "OxygenCandle"},
        {"CF", "ForceField"},
        {"CG", "Missile"},
        {"CH", "Television"},
        {"CI", "Magnesium"},
        {"CJ", "Sand"},
        {"CK", "Asphalt"},
        {"CL", "Gunpowder"},
        {"CM", "Ashes"},
        {"CN", "TouchTrigger"},
        {"CO", "ZapWire"},
        {"CP", "Ruby"},
        {"CQ", "Explosives"},
        {"CR", "RemoteSeat"},
        {"CS", "Quartz"},
        {"CT", "SolarPanel"},
        {"CU", "Speaker"},
        {"CV", "PowerCell"},
        {"CW", "Container"},
        {"CX", "Inverter"},
        {"CY", "Heater"},
        {"CZ", "Wire"},
        {"DA", "Sign"},
        {"DB", "Switch"},
        {"DC", "Light"},
        {"DD", "Generator"},
        {"DE", "SignalWire"},
        {"DF", "TintedGlass"},
        {"DG", "Polysilicon"},
        {"DH", "Balloon"},
        {"DI", "Truss"},
        {"DJ", "Firework"},
        {"DK", "Faucet"},
        {"DL", "ExoticMatter"},
        {"DM", "PhotonShard"},
        {"DN", "GlowTube"},
        {"DO", "Cannon"},
        {"DP", "Treads"},
        {"DQ", "Motor"},
        {"DR", "Propeller"},
        {"DS", "Converter"},
        {"DT", "Pipe"},
        {"DU", "Lift"},
        {"DV", "EnrichedUranium"},
        {"DW", "SignalSwitch"},
        {"DX", "Oil"},
        {"DY", "EMPBomb"},
        {"DZ", "Coat"},
        {"EA", "PolyesterCoat"},
        {"EB", "Engine"},
        {"EC", "InertiaTrigger"},
        {"ED", "GammaDrive"},
        {"EE", "Warhead"},
        {"EF", "Reactor"},
        {"EG", "SpawnPoint"},
        {"EH", "Rocket"},
        {"EI", "Mercury"},
        {"EJ", "Camera"},
        {"EK", "SpaceSuit"},
        {"EL", "Protalium"},
        {"EM", "ProtonCell"},
        {"EN", "WedgeTemplate"},
        {"EO", "Compressor"},
        {"EP", "WheelTemplate"},
        {"EQ", "CloudSeeder"},
        {"ER", "Factory"},
        {"ES", "Experimental"},
        {"ET", "Ore"},
        {"EU", "Trampoline"},
        {"EV", "BaseCenter"},
        {"EW", "Seeker"},
        {"EX", "HyperDrive"},
        {"EY", "Prospector"},
        {"EZ", "SteamEngine"},
        {"FA", "Lirvanite"},
        {"FB", "Endgame"},
        {"FC", "TimeMachine"},
        {"FD", "Pod"},
        {"FE", "BioWall"},
        {"FF", "Clock"},
        {"FG", "BallastTank"},
        {"FH", "Keyboard"},
        {"FI", "CloakingDevice"},
        {"FJ", "AlkalineCell"},
        {"FK", "Commlink"},
        {"FL", "Bogie"},
        {"FM", "RadiationSuit"},
        {"FN", "Sulfur"},
        {"FO", "ActiveDefense"},
        {"FP", "Curium"},
        {"FQ", "AetherGate"},
        {"FR", "Americium"},
        {"FS", "Airbag"},
        {"FT", "ComboLock"},
        {"FU", "Lab"},
        {"FV", "PowerPlant"},
        {"FW", "Teleporter"},
        {"FX", "Goo"},
        {"FY", "BallTemplate"},
        {"FZ", "BladeTemplate"},
        {"GA", "Hull"},
        {"GB", "IonRocket"},
        {"GC", "IonDrive"},
        {"GD", "LaserTargeter"},
        {"GE", "Actuator"},
        {"GF", "Node"},
        {"GG", "Valve"},
        {"GH", "MemoryChip"},
        {"GI", "Monitor"},
        {"GJ", "Scanner"},
        {"GK", "Mainspring"},
        {"GL", "StirlingEngine"},
        {"GM", "Coal"},
        {"GN", "Rare"},
        {"GO", "SearchLight"},
        {"GP", "Computer"},
        {"GQ", "EnergyShield"},
        {"GR", "Accelerator"},
        {"GS", "EnergyBomb"},
        {"GT", "EnergyCannon"},
        {"GU", "CornerTemplate"},
        {"GV", "Abantium"},
        {"GW", "Armor"},
        {"GX", "Rotor"},
        {"GY", "Belt"},
        {"GZ", "BotSpawn"},
        {"HA", "Lexan"},
        {"HB", "Zaktralia"},
        {"HC", "Firewood"},
        {"HD", "ProbabilityField"},
        {"HE", "Foam"},
        {"HF", "FlameThrower"},
        {"HG", "Launcher"},
        {"HH", "Sensor"},
        {"HI", "GraphicsCard"},
        {"HJ", "Cloth"},
        {"HK", "Ceramic"},
        {"HL", "Hybrid"},
        {"HM", "Ramjet"},
        {"HN", "Ammo"},
        {"HO", "Dish"},
        {"HP", "Button"},
        {"HQ", "SingularityBomb"},
        {"HR", "Radar"},
        {"HS", "Gel"},
        {"HT", "Trinket"},
        {"HU", "Plant"},
        {"HV", "Pulsejet"},
        {"HW", "Inductor"},
        {"HX", "Firebox"},
        {"HY", "LithiumCell"},
        {"HZ", "Gun"},
        {"IA", "Book"},
        {"IB", "Track"},
        {"IC", "Bend"},
        {"ID", "Market"},
        {"IE", "WarpGate"},
        {"IF", "Gas"},
        {"IG", "ParachutePack"},
        {"IH", "Seed"},
        {"II", "Bank"},
        {"IJ", "Filler"},
        {"IK", "Charcoal"},
        {"IL", "Wood"},
        {"IM", "Interceptor"},
        {"IN", "Windmill"},
        {"IO", "BilgePump"},
        {"IP", "CrudeHull"},
        {"IQ", "SeaAlloy"},
        {"IR", "Catapult"},
        {"IS", "BioLab"},
        {"IT", "Cotton"},
        {"IU", "SelfDestruct"},
        {"IV", "Derrick"},
        {"IW", "ConfigBox"},
        {"IX", "Plutonium"},
        {"IY", "DeuteriumCap"},
        {"IZ", "LCD"},
        {"JA", "Radio"},
        {"JB", "Horse"},
        {"JC", "Crate"},
        {"JD", "Flubber"},
        {"JE", "Brake"},
        {"JF", "Foundation"},
        {"JG", "Concrete"},
        {"JH", "ScubaSuit"},
        {"JI", "BeamCannon"},
        {"JJ", "PayBox"},
        {"JK", "Turboshaft"},
        {"JL", "PulseDrive"},
        {"JM", "SpeedWalk"},
        {"JN", "Y"},
        {"JO", "Respirator"},
        {"JP", "Controller"},
        {"JQ", "HazmatSuit"},
        {"JR", "Mapper"},
        {"JS", "Resonator"},
        {"JT", "Aluminium"},
        {"JU", "DelayWire"},
        {"JV", "BarbedWire"},
        {"JW", "ChemicalPlant"},
        {"JX", "QuantumAntenna"},
        {"JY", "Keypad"},
        {"JZ", "TelePipe"},
        {"KA", "Sandstone"},
        {"KB", "Teflon"},
        {"KC", "ZapCoil"},
        {"KD", "Recycler"},
        {"KE", "Builder"},
        {"KF", "SeaPropeller"},
        {"KG", "PhotoCamera"},
        {"KH", "Photograph"},
        {"KI", "Thermite"},
        {"KJ", "KillFloor"},
        {"KK", "FuelCell"},
        {"KL", "PlateTemplate"},
        {"KM", "BurstWire"},
        {"KN", "Funium"},
        {"KO", "Detector"},
        {"KP", "ICBM"},
        {"KQ", "Transmission"},
        {"KR", "Steel"},
        {"KS", "TorqueSpring"},
        {"KT", "Substation"},
        {"KU", "Conduit"},
        {"KV", "InsulatedWire"},
        {"KW", "Egg"},
        {"KX", "Wasp"},
        {"KY", "Sentry"},
        {"KZ", "FaxRadio"}
    };
    Dictionary<int, Color> colours = new Dictionary<int, Color>()
    {
        {1, new Color(0.950f, 0.953f, 0.953f)},
        {2, new Color(0.632f, 0.648f, 0.636f)},
        {3, new Color(0.977f, 0.914f, 0.600f)},
        {5, new Color(0.844f, 0.773f, 0.604f)},
        {6, new Color(0.761f, 0.855f, 0.722f)},
        {9, new Color(0.910f, 0.730f, 0.785f)},
        {11, new Color(0.502f, 0.734f, 0.859f)},
        {12, new Color(0.797f, 0.518f, 0.259f)},
        {18, new Color(0.800f, 0.557f, 0.412f)},
        {21, new Color(0.769f, 0.157f, 0.110f)},
        {22, new Color(0.769f, 0.440f, 0.628f)},
        {23, new Color(0.051f, 0.412f, 0.675f)},
        {24, new Color(0.961f, 0.804f, 0.189f)},
        {25, new Color(0.385f, 0.279f, 0.197f)},
        {26, new Color(0.106f, 0.165f, 0.208f)},
        {27, new Color(0.428f, 0.432f, 0.424f)},
        {28, new Color(0.157f, 0.499f, 0.279f)},
        {29, new Color(0.632f, 0.769f, 0.550f)},
        {36, new Color(0.953f, 0.812f, 0.608f)},
        {37, new Color(0.295f, 0.593f, 0.295f)},
        {38, new Color(0.628f, 0.373f, 0.208f)},
        {39, new Color(0.757f, 0.793f, 0.871f)},
        {40, new Color(0.926f, 0.926f, 0.926f)},
        {41, new Color(0.804f, 0.330f, 0.295f)},
        {42, new Color(0.757f, 0.875f, 0.942f)},
        {43, new Color(0.483f, 0.714f, 0.910f)},
        {44, new Color(0.969f, 0.946f, 0.553f)},
        {45, new Color(0.706f, 0.824f, 0.895f)},
        {47, new Color(0.851f, 0.522f, 0.424f)},
        {48, new Color(0.518f, 0.714f, 0.553f)},
        {49, new Color(0.973f, 0.946f, 0.518f)},
        {50, new Color(0.926f, 0.910f, 0.871f)},
        {100, new Color(0.934f, 0.769f, 0.714f)},
        {101, new Color(0.855f, 0.526f, 0.479f)},
        {102, new Color(0.432f, 0.600f, 0.793f)},
        {103, new Color(0.781f, 0.757f, 0.718f)},
        {104, new Color(0.420f, 0.197f, 0.487f)},
        {105, new Color(0.887f, 0.608f, 0.251f)},
        {106, new Color(0.855f, 0.522f, 0.255f)},
        {107, new Color(0.000f, 0.561f, 0.612f)},
        {108, new Color(0.408f, 0.361f, 0.263f)},
        {110, new Color(0.263f, 0.330f, 0.577f)},
        {111, new Color(0.750f, 0.718f, 0.695f)},
        {112, new Color(0.408f, 0.455f, 0.675f)},
        {113, new Color(0.899f, 0.679f, 0.785f)},
        {115, new Color(0.781f, 0.824f, 0.236f)},
        {116, new Color(0.334f, 0.648f, 0.687f)},
        {118, new Color(0.718f, 0.844f, 0.836f)},
        {119, new Color(0.644f, 0.742f, 0.279f)},
        {120, new Color(0.851f, 0.895f, 0.655f)},
        {121, new Color(0.906f, 0.675f, 0.346f)},
        {123, new Color(0.828f, 0.436f, 0.299f)},
        {124, new Color(0.573f, 0.224f, 0.471f)},
        {125, new Color(0.918f, 0.722f, 0.573f)},
        {126, new Color(0.648f, 0.648f, 0.797f)},
        {127, new Color(0.863f, 0.738f, 0.506f)},
        {128, new Color(0.683f, 0.479f, 0.350f)},
        {131, new Color(0.612f, 0.640f, 0.659f)},
        {133, new Color(0.836f, 0.451f, 0.240f)},
        {134, new Color(0.848f, 0.867f, 0.338f)},
        {135, new Color(0.455f, 0.526f, 0.616f)},
        {136, new Color(0.530f, 0.487f, 0.565f)},
        {137, new Color(0.879f, 0.597f, 0.393f)},
        {138, new Color(0.585f, 0.542f, 0.451f)},
        {140, new Color(0.126f, 0.228f, 0.338f)},
        {141, new Color(0.153f, 0.275f, 0.177f)},
        {143, new Color(0.812f, 0.887f, 0.969f)},
        {145, new Color(0.475f, 0.534f, 0.632f)},
        {146, new Color(0.585f, 0.557f, 0.640f)},
        {147, new Color(0.577f, 0.530f, 0.404f)},
        {148, new Color(0.342f, 0.346f, 0.342f)},
        {149, new Color(0.087f, 0.114f, 0.197f)},
        {150, new Color(0.671f, 0.679f, 0.675f)},
        {151, new Color(0.471f, 0.565f, 0.510f)},
        {153, new Color(0.585f, 0.475f, 0.467f)},
        {154, new Color(0.483f, 0.181f, 0.185f)},
        {157, new Color(1.000f, 0.965f, 0.483f)},
        {158, new Color(0.883f, 0.644f, 0.761f)},
        {168, new Color(0.459f, 0.424f, 0.385f)},
        {176, new Color(0.593f, 0.412f, 0.357f)},
        {178, new Color(0.706f, 0.518f, 0.334f)},
        {179, new Color(0.538f, 0.530f, 0.534f)},
        {180, new Color(0.844f, 0.663f, 0.295f)},
        {190, new Color(0.977f, 0.840f, 0.181f)},
        {191, new Color(0.910f, 0.671f, 0.177f)},
        {192, new Color(0.412f, 0.251f, 0.157f)},
        {193, new Color(0.812f, 0.377f, 0.142f)},
        {194, new Color(0.640f, 0.636f, 0.648f)},
        {195, new Color(0.275f, 0.404f, 0.644f)},
        {196, new Color(0.138f, 0.279f, 0.546f)},
        {198, new Color(0.557f, 0.259f, 0.522f)},
        {199, new Color(0.389f, 0.373f, 0.385f)},
        {200, new Color(0.510f, 0.542f, 0.365f)},
        {208, new Color(0.899f, 0.895f, 0.875f)},
        {209, new Color(0.691f, 0.557f, 0.267f)},
        {210, new Color(0.440f, 0.585f, 0.471f)},
        {211, new Color(0.475f, 0.710f, 0.710f)},
        {212, new Color(0.624f, 0.765f, 0.914f)},
        {213, new Color(0.424f, 0.506f, 0.718f)},
        {216, new Color(0.565f, 0.299f, 0.165f)},
        {217, new Color(0.487f, 0.361f, 0.275f)},
        {218, new Color(0.589f, 0.440f, 0.624f)},
        {219, new Color(0.420f, 0.385f, 0.608f)},
        {220, new Color(0.655f, 0.663f, 0.808f)},
        {221, new Color(0.804f, 0.385f, 0.597f)},
        {222, new Color(0.895f, 0.679f, 0.785f)},
        {223, new Color(0.863f, 0.565f, 0.585f)},
        {224, new Color(0.942f, 0.836f, 0.628f)},
        {225, new Color(0.922f, 0.722f, 0.499f)},
        {226, new Color(0.993f, 0.918f, 0.553f)},
        {232, new Color(0.491f, 0.734f, 0.867f)},
        {268, new Color(0.204f, 0.169f, 0.459f)},
        {301, new Color(0.314f, 0.428f, 0.330f)},
        {302, new Color(0.357f, 0.365f, 0.412f)},
        {303, new Color(0.000f, 0.063f, 0.691f)},
        {304, new Color(0.173f, 0.397f, 0.114f)},
        {305, new Color(0.322f, 0.487f, 0.683f)},
        {306, new Color(0.200f, 0.346f, 0.510f)},
        {307, new Color(0.063f, 0.165f, 0.863f)},
        {308, new Color(0.240f, 0.083f, 0.522f)},
        {309, new Color(0.204f, 0.557f, 0.251f)},
        {310, new Color(0.357f, 0.604f, 0.299f)},
        {311, new Color(0.624f, 0.632f, 0.675f)},
        {312, new Color(0.350f, 0.134f, 0.350f)},
        {313, new Color(0.122f, 0.502f, 0.114f)},
        {314, new Color(0.624f, 0.679f, 0.753f)},
        {315, new Color(0.036f, 0.538f, 0.812f)},
        {316, new Color(0.483f, 0.000f, 0.483f)},
        {317, new Color(0.487f, 0.612f, 0.420f)},
        {318, new Color(0.542f, 0.671f, 0.522f)},
        {319, new Color(0.726f, 0.769f, 0.695f)},
        {320, new Color(0.793f, 0.797f, 0.820f)},
        {321, new Color(0.655f, 0.369f, 0.608f)},
        {322, new Color(0.483f, 0.185f, 0.483f)},
        {323, new Color(0.581f, 0.746f, 0.506f)},
        {324, new Color(0.659f, 0.742f, 0.600f)},
        {325, new Color(0.875f, 0.875f, 0.871f)},
        {327, new Color(0.593f, 0.000f, 0.000f)},
        {328, new Color(0.695f, 0.899f, 0.651f)},
        {329, new Color(0.597f, 0.761f, 0.859f)},
        {330, new Color(1.000f, 0.597f, 0.863f)},
        {331, new Color(1.000f, 0.350f, 0.350f)},
        {332, new Color(0.459f, 0.000f, 0.000f)},
        {333, new Color(0.938f, 0.722f, 0.220f)},
        {334, new Color(0.973f, 0.851f, 0.428f)},
        {335, new Color(0.906f, 0.906f, 0.926f)},
        {336, new Color(0.781f, 0.832f, 0.895f)},
        {337, new Color(1.000f, 0.581f, 0.581f)},
        {338, new Color(0.746f, 0.408f, 0.385f)},
        {339, new Color(0.338f, 0.142f, 0.142f)},
        {340, new Color(0.946f, 0.906f, 0.781f)},
        {341, new Color(0.997f, 0.953f, 0.734f)},
        {342, new Color(0.879f, 0.699f, 0.816f)},
        {343, new Color(0.832f, 0.565f, 0.742f)},
        {344, new Color(0.589f, 0.334f, 0.334f)},
        {345, new Color(0.561f, 0.299f, 0.165f)},
        {346, new Color(0.828f, 0.746f, 0.589f)},
        {347, new Color(0.887f, 0.863f, 0.738f)},
        {348, new Color(0.930f, 0.918f, 0.918f)},
        {349, new Color(0.914f, 0.855f, 0.855f)},
        {350, new Color(0.534f, 0.244f, 0.244f)},
        {351, new Color(0.738f, 0.608f, 0.365f)},
        {352, new Color(0.781f, 0.675f, 0.471f)},
        {353, new Color(0.793f, 0.750f, 0.640f)},
        {354, new Color(0.734f, 0.702f, 0.699f)},
        {355, new Color(0.424f, 0.346f, 0.295f)},
        {356, new Color(0.628f, 0.518f, 0.310f)},
        {357, new Color(0.585f, 0.538f, 0.534f)},
        {358, new Color(0.671f, 0.659f, 0.620f)},
        {359, new Color(0.687f, 0.581f, 0.514f)},
        {360, new Color(0.589f, 0.404f, 0.400f)},
        {361, new Color(0.338f, 0.259f, 0.212f)},
        {362, new Color(0.495f, 0.408f, 0.248f)},
        {363, new Color(0.412f, 0.400f, 0.361f)},
        {364, new Color(0.353f, 0.299f, 0.259f)},
        {365, new Color(0.416f, 0.224f, 0.036f)},
        {1001, new Color(0.973f, 0.973f, 0.973f)},
        {1002, new Color(0.804f, 0.804f, 0.804f)},
        {1003, new Color(0.067f, 0.067f, 0.067f)},
        {1004, new Color(1.000f, 0.000f, 0.000f)},
        {1005, new Color(1.000f, 0.691f, 0.000f)},
        {1006, new Color(0.706f, 0.502f, 1.000f)},
        {1007, new Color(0.640f, 0.295f, 0.295f)},
        {1008, new Color(0.757f, 0.746f, 0.259f)},
        {1009, new Color(1.000f, 1.000f, 0.000f)},
        {1010, new Color(0.000f, 0.000f, 1.000f)},
        {1011, new Color(0.000f, 0.126f, 0.377f)},
        {1012, new Color(0.130f, 0.330f, 0.726f)},
        {1013, new Color(0.016f, 0.687f, 0.926f)},
        {1014, new Color(0.667f, 0.334f, 0.000f)},
        {1015, new Color(0.667f, 0.000f, 0.667f)},
        {1016, new Color(1.000f, 0.400f, 0.800f)},
        {1017, new Color(1.000f, 0.687f, 0.000f)},
        {1018, new Color(0.071f, 0.934f, 0.832f)},
        {1019, new Color(0.000f, 1.000f, 1.000f)},
        {1020, new Color(0.000f, 1.000f, 0.000f)},
        {1021, new Color(0.228f, 0.491f, 0.083f)},
        {1022, new Color(0.499f, 0.557f, 0.393f)},
        {1023, new Color(0.550f, 0.357f, 0.624f)},
        {1024, new Color(0.687f, 0.867f, 1.000f)},
        {1025, new Color(1.000f, 0.789f, 0.789f)},
        {1026, new Color(0.695f, 0.655f, 1.000f)},
        {1027, new Color(0.624f, 0.953f, 0.914f)},
        {1028, new Color(0.800f, 1.000f, 0.800f)},
        {1029, new Color(1.000f, 1.000f, 0.800f)},
        {1030, new Color(1.000f, 0.800f, 0.600f)},
        {1031, new Color(0.385f, 0.146f, 0.820f)},
        {1032, new Color(1.000f, 0.000f, 0.750f)}
    };
    Dictionary<string, Vector3> defaultSizes = new Dictionary<string, Vector3>()
    {
        {"HS", new Vector3(2f, 2f, 2f)},
        {"FI", new Vector3(2f, 2f, 2f)},
        {"EH", new Vector3(4f, 6f, 4f)},
        {"HP", new Vector3(2f, 1f, 2f)},
        {"JE", new Vector3(2f, 1f, 2f)},
        {"HG", new Vector3(2f, 5f, 2f)},
        {"HL", new Vector3(2f, 2f, 2f)},
        {"HK", new Vector3(4f, 1f, 4f)},
        {"HJ", new Vector3(4f, 0.2f, 4f)},
        {"ED", new Vector3(4f, 4f, 4f)},
        {"HM", new Vector3(2f, 10f, 2f)},
        {"GT", new Vector3(10f, 3f, 3f)},
        {"AT", new Vector3(24f, 24f, 6f)},
        {"HE", new Vector3(4f, 2f, 4f)},
        {"HD", new Vector3(4f, 4f, 1f)},
        {"HC", new Vector3(4f, 2f, 2f)},
        {"JV", new Vector3(1f, 1f, 8f)},
        {"HA", new Vector3(6f, 4f, 1f)},
        {"AE", new Vector3(4f, 4f, 4f)},
        {"BZ", new Vector3(2f, 1f, 2f)},
        {"GX", new Vector3(2f, 46f, 1f)},
        {"GW", new Vector3(2f, 2f, 1f)},
        {"GV", new Vector3(4f, 2f, 2f)},
        {"GU", new Vector3(2f, 2f, 2f)},
        {"GQ", new Vector3(4f, 6f, 4f)},
        {"AF", new Vector3(2f, 2f, 2f)},
        {"DY", new Vector3(2f, 4f, 2f)},
        {"AR", new Vector3(3f, 2f, 2f)},
        {"IU", new Vector3(2f, 3f, 3f)},
        {"GO", new Vector3(4f, 6f, 4f)},
        {"AO", new Vector3(4f, 4f, 4f)},
        {"IK", new Vector3(1f, 1f, 1f)},
        {"IB", new Vector3(10f, 1f, 20f)},
        {"HN", new Vector3(3f, 4f, 3f)},
        {"GJ", new Vector3(4f, 4f, 4f)},
        {"FL", new Vector3(10f, 3f, 8f)},
        {"GH", new Vector3(2f, 1f, 2f)},
        {"GG", new Vector3(2f, 1f, 3f)},
        {"EG", new Vector3(4f, 1f, 4f)},
        {"CQ", new Vector3(2f, 2f, 2f)},
        {"JI", new Vector3(2f, 8f, 2f)},
        {"DP", new Vector3(2f, 2f, 6f)},
        {"GB", new Vector3(2f, 6f, 2f)},
        {"ID", new Vector3(8f, 14f, 8f)},
        {"FZ", new Vector3(2f, 2f, 2f)},
        {"FY", new Vector3(2f, 2f, 2f)},
        {"FX", new Vector3(3f, 1f, 3f)},
        {"BI", new Vector3(4f, 6f, 4f)},
        {"HQ", new Vector3(3f, 6f, 3f)},
        {"GK", new Vector3(1f, 2f, 2f)},
        {"GI", new Vector3(4f, 4f, 4f)},
        {"AJ", new Vector3(4f, 2f, 2f)},
        {"AL", new Vector3(2f, 1f, 3f)},
        {"AA", new Vector3(2f, 1f, 2f)},
        {"BN", new Vector3(12f, 1f, 3f)},
        {"AH", new Vector3(1f, 6f, 4f)},
        {"FN", new Vector3(1f, 3f, 1f)},
        {"FM", new Vector3(2f, 2f, 1f)},
        {"HH", new Vector3(2f, 1f, 1f)},
        {"FK", new Vector3(2f, 1f, 2f)},
        {"JO", new Vector3(2f, 4f, 2f)},
        {"IS", new Vector3(6f, 6f, 6f)},
        {"GP", new Vector3(4f, 2f, 4f)},
        {"FG", new Vector3(6f, 4f, 4f)},
        {"FP", new Vector3(1f, 1f, 1f)},
        {"FJ", new Vector3(1f, 2f, 1f)},
        {"FD", new Vector3(4f, 6f, 4f)},
        {"JR", new Vector3(3f, 6f, 3f)},
        {"AK", new Vector3(2f, 2f, 2f)},
        {"FA", new Vector3(5f, 2f, 1f)},
        {"EZ", new Vector3(4f, 4f, 6f)},
        {"BH", new Vector3(2f, 2f, 2f)},
        {"FH", new Vector3(4f, 1f, 2f)},
        {"EW", new Vector3(2f, 2f, 2f)},
        {"EV", new Vector3(10f, 10f, 10f)},
        {"HO", new Vector3(10f, 2f, 10f)},
        {"AP", new Vector3(2f, 1f, 2f)},
        {"AG", new Vector3(1f, 2f, 1f)},
        {"GS", new Vector3(2f, 6f, 2f)},
        {"AI", new Vector3(4f, 2f, 2f)},
        {"EP", new Vector3(1f, 4f, 4f)},
        {"JX", new Vector3(2f, 2f, 2f)},
        {"EN", new Vector3(2f, 2f, 2f)},
        {"EM", new Vector3(4f, 6f, 4f)},
        {"EL", new Vector3(1f, 4f, 1f)},
        {"EK", new Vector3(2f, 2f, 1f)},
        {"EJ", new Vector3(1f, 1f, 2f)},
        {"AQ", new Vector3(4f, 2f, 2f)},
        {"BW", new Vector3(4f, 4f, 4f)},
        {"AN", new Vector3(4f, 4f, 1f)},
        {"CV", new Vector3(2f, 2f, 2f)},
        {"FV", new Vector3(8f, 6f, 10f)},
        {"ER", new Vector3(10f, 10f, 10f)},
        {"GC", new Vector3(8f, 10f, 8f)},
        {"EB", new Vector3(2f, 2f, 4f)},
        {"EA", new Vector3(2f, 2f, 1f)},
        {"DZ", new Vector3(2f, 2f, 1f)},
        {"FU", new Vector3(6f, 6f, 6f)},
        {"AM", new Vector3(2f, 3f, 2f)},
        {"DW", new Vector3(2f, 1f, 2f)},
        {"DV", new Vector3(2f, 2f, 2f)},
        {"DU", new Vector3(5f, 4f, 1f)},
        {"DT", new Vector3(8f, 1f, 1f)},
        {"DS", new Vector3(6f, 6f, 6f)},
        {"HB", new Vector3(6f, 1f, 2f)},
        {"DQ", new Vector3(2f, 2f, 3f)},
        {"FC", new Vector3(6f, 6f, 6f)},
        {"DO", new Vector3(2f, 8f, 2f)},
        {"JP", new Vector3(4f, 4f, 1f)},
        {"DM", new Vector3(1f, 2f, 2f)},
        {"DL", new Vector3(1f, 1f, 1f)},
        {"DK", new Vector3(1f, 2f, 1f)},
        {"DJ", new Vector3(2f, 2f, 2f)},
        {"DI", new Vector3(2f, 6f, 2f)},
        {"DH", new Vector3(4f, 4f, 4f)},
        {"DG", new Vector3(2f, 1f, 1f)},
        {"DF", new Vector3(4f, 4f, 1f)},
        {"DE", new Vector3(8f, 1f, 1f)},
        {"FF", new Vector3(6f, 1f, 6f)},
        {"DC", new Vector3(2f, 2f, 2f)},
        {"DB", new Vector3(2f, 1f, 2f)},
        {"DA", new Vector3(4f, 2f, 1f)},
        {"CZ", new Vector3(8f, 1f, 1f)},
        {"CY", new Vector3(4f, 2f, 2f)},
        {"CX", new Vector3(3f, 2f, 3f)},
        {"CW", new Vector3(3f, 3f, 3f)},
        {"HF", new Vector3(1f, 8f, 1f)},
        {"CU", new Vector3(2f, 2f, 2f)},
        {"CT", new Vector3(6f, 1f, 6f)},
        {"CS", new Vector3(3f, 3f, 3f)},
        {"CR", new Vector3(2f, 1f, 2f)},
        {"HZ", new Vector3(1f, 5f, 1f)},
        {"CP", new Vector3(2f, 1f, 1f)},
        {"CO", new Vector3(8f, 1f, 1f)},
        {"CN", new Vector3(2f, 1f, 2f)},
        {"CM", new Vector3(2f, 1f, 2f)},
        {"CL", new Vector3(2f, 1f, 2f)},
        {"CK", new Vector3(8f, 1f, 8f)},
        {"CJ", new Vector3(2f, 1f, 2f)},
        {"CI", new Vector3(2f, 1f, 2f)},
        {"DD", new Vector3(2f, 2f, 2f)},
        {"CG", new Vector3(1f, 4f, 1f)},
        {"CF", new Vector3(10f, 1f, 10f)},
        {"GF", new Vector3(4f, 4f, 4f)},
        {"CD", new Vector3(6f, 6f, 6f)},
        {"CC", new Vector3(1f, 4f, 1f)},
        {"CB", new Vector3(2f, 1f, 2f)},
        {"CA", new Vector3(2f, 1f, 2f)},
        {"HW", new Vector3(3f, 2f, 2f)},
        {"BY", new Vector3(1f, 20f, 1f)},
        {"BX", new Vector3(2f, 1f, 2f)},
        {"FT", new Vector3(2f, 2f, 2f)},
        {"KI", new Vector3(2f, 1f, 2f)},
        {"BU", new Vector3(3f, 1f, 3f)},
        {"BT", new Vector3(2f, 2f, 2f)},
        {"BS", new Vector3(6f, 1f, 6f)},
        {"BR", new Vector3(2f, 2f, 2f)},
        {"FW", new Vector3(8f, 2f, 8f)},
        {"BP", new Vector3(4f, 4f, 1f)},
        {"IL", new Vector3(6f, 2f, 2f)},
        {"BQ", new Vector3(1f, 12f, 3f)},
        {"BM", new Vector3(4f, 2f, 2f)},
        {"JU", new Vector3(2f, 1f, 2f)},
        {"BK", new Vector3(2f, 1f, 2f)},
        {"BJ", new Vector3(4f, 2f, 2f)},
        {"EE", new Vector3(4f, 6f, 4f)},
        {"EF", new Vector3(10f, 10f, 10f)},
        {"BG", new Vector3(4f, 1f, 4f)},
        {"BF", new Vector3(4f, 1f, 4f)},
        {"BE", new Vector3(10f, 10f, 1f)},
        {"JT", new Vector3(4f, 2f, 2f)},
        {"BC", new Vector3(2f, 1f, 2f)},
        {"BB", new Vector3(4f, 2f, 3f)},
        {"BA", new Vector3(1f, 1f, 1f)},
        {"AZ", new Vector3(2f, 1f, 2f)},
        {"AY", new Vector3(4f, 1f, 2f)},
        {"AX", new Vector3(2f, 2f, 2f)},
        {"AW", new Vector3(1f, 2f, 1f)},
        {"AV", new Vector3(1f, 4f, 1f)},
        {"AU", new Vector3(4f, 1f, 4f)},
        {"EC", new Vector3(2f, 2f, 2f)},
        {"AS", new Vector3(2f, 1f, 2f)},
        {"HR", new Vector3(12f, 3f, 12f)},
        {"IC", new Vector3(1f, 20f, 20f)},
        {"HX", new Vector3(4f, 4f, 4f)},
        {"BL", new Vector3(2f, 1f, 2f)},
        {"AC", new Vector3(2f, 2f, 11f)},
        {"FE", new Vector3(4f, 2f, 4f)},
        {"FO", new Vector3(3f, 6f, 3f)},
        {"GA", new Vector3(16f, 4f, 6f)},
        {"GD", new Vector3(2f, 2f, 2f)},
        {"JQ", new Vector3(2f, 2f, 1f)},
        {"GR", new Vector3(8f, 30f, 30f)},
        {"HI", new Vector3(2f, 1f, 2f)},
        {"IA", new Vector3(1f, 3f, 2f)},
        {"IG", new Vector3(2f, 2f, 1f)},
        {"IJ", new Vector3(4f, 4f, 2f)},
        {"IM", new Vector3(3f, 3f, 20f)},
        {"IN", new Vector3(16f, 12f, 12f)},
        {"IO", new Vector3(3f, 3f, 3f)},
        {"IP", new Vector3(16f, 4f, 6f)},
        {"IQ", new Vector3(4f, 2f, 2f)},
        {"IR", new Vector3(2f, 2f, 8f)},
        {"IT", new Vector3(1f, 1f, 1f)},
        {"CH", new Vector3(3f, 3f, 3f)},
        {"IV", new Vector3(8f, 10f, 8f)},
        {"EY", new Vector3(6f, 1f, 6f)},
        {"JD", new Vector3(3f, 3f, 3f)},
        {"JG", new Vector3(4f, 1f, 4f)},
        {"IY", new Vector3(1f, 2f, 2f)},
        {"GY", new Vector3(6f, 2f, 12f)},
        {"IX", new Vector3(2f, 1f, 2f)},
        {"DN", new Vector3(1f, 2f, 1f)},
        {"JF", new Vector3(10f, 1f, 10f)},
        {"BO", new Vector3(2f, 1f, 2f)},
        {"JA", new Vector3(2f, 1f, 2f)},
        {"IZ", new Vector3(4f, 4f, 1f)},
        {"JB", new Vector3(8f, 3f, 2f)},
        {"JC", new Vector3(4f, 2f, 4f)},
        {"JH", new Vector3(2f, 2f, 1f)},
        {"JK", new Vector3(4f, 6f, 4f)},
        {"JL", new Vector3(4f, 14f, 4f)},
        {"JJ", new Vector3(2f, 2f, 4f)},
        {"EU", new Vector3(8f, 1f, 8f)},
        {"JM", new Vector3(8f, 1f, 8f)},
        {"JN", new Vector3(6f, 4f, 6f)},
        {"GZ", new Vector3(4f, 1f, 4f)},
        {"DR", new Vector3(2f, 0.4f, 8f)},
        {"EO", new Vector3(4f, 6f, 2f)},
        {"GL", new Vector3(2f, 2f, 4f)},
        {"IW", new Vector3(2f, 1f, 1f)},
        {"JS", new Vector3(2f, 1f, 2f)},
        {"BD", new Vector3(4f, 2f, 2f)},
        {"HY", new Vector3(2f, 1f, 2f)},
        {"EX", new Vector3(6f, 6f, 20f)},
        {"IE", new Vector3(30f, 30f, 2f)},
        {"JW", new Vector3(8f, 10f, 8f)},
        {"CE", new Vector3(2f, 4f, 2f)},
        {"GE", new Vector3(4f, 1f, 4f)},
        {"JY", new Vector3(2f, 3f, 1f)},
        {"KB", new Vector3(2f, 2f, 2f)},
        {"KA", new Vector3(3f, 3f, 4f)},
        {"JZ", new Vector3(1f, 4f, 1f)},
        {"KC", new Vector3(6f, 6f, 6f)},
        {"KD", new Vector3(6f, 6f, 6f)},
        {"EQ", new Vector3(6f, 10f, 6f)},
        {"AB", new Vector3(2f, 2f, 1f)},
        {"HV", new Vector3(2f, 12f, 2f)},
        {"KE", new Vector3(4f, 2f, 2f)},
        {"FR", new Vector3(2f, 1f, 1f)},
        {"FS", new Vector3(2f, 1f, 2f)},
        {"II", new Vector3(4f, 4f, 4f)},
        {"KF", new Vector3(1f, 2f, 4f)},
        {"KJ", new Vector3(8f, 1f, 8f)},
        {"KH", new Vector3(4f, 4f, 1f)},
        {"KG", new Vector3(2f, 2f, 2f)},
        {"KK", new Vector3(4f, 1f, 4f)},
        {"BV", new Vector3(2f, 3f, 2f)},
        {"KL", new Vector3(4f, 0.2f, 4f)},
        {"KM", new Vector3(6f, 1f, 1f)},
        {"KO", new Vector3(4f, 1f, 4f)},
        {"KN", new Vector3(4f, 2f, 2f)},
        {"KP", new Vector3(6f, 30f, 6f)},
        {"KQ", new Vector3(4f, 2f, 6f)},
        {"KR", new Vector3(4f, 2f, 2f)},
        {"KS", new Vector3(2f, 1f, 2f)},
        {"KT", new Vector3(10f, 6f, 10f)},
        {"KU", new Vector3(2f, 1f, 2f)},
        {"KV", new Vector3(8f, 1f, 1f)},
        {"KZ", new Vector3(6f, 2f, 4f)}
    };
    Dictionary<string, int> defaultColours = new Dictionary<string, int>()
    {
        {"HS", 1019},
        {"FI", 311},
        {"EH", 302},
        {"HP", 106},
        {"JE", 1003},
        {"HG", 352},
        {"HL", 320},
        {"HK", 1001},
        {"HJ", 346},
        {"ED", 1020},
        {"HM", 357},
        {"GT", 199},
        {"AT", 194},
        {"HE", 1001},
        {"HD", 1001},
        {"HC", 192},
        {"JV", 363},
        {"HA", 1001},
        {"AE", 302},
        {"BZ", 28},
        {"GX", 1003},
        {"GW", 320},
        {"GV", 1001},
        {"GU", 348},
        {"GQ", 1003},
        {"AF", 1024},
        {"DY", 194},
        {"AR", 194},
        {"IU", 1003},
        {"GO", 26},
        {"AO", 21},
        {"IK", 1003},
        {"IB", 363},
        {"HN", 359},
        {"GJ", 1003},
        {"FL", 199},
        {"GH", 304},
        {"GG", 311},
        {"EG", 194},
        {"CQ", 21},
        {"JI", 194},
        {"DP", 199},
        {"GB", 325},
        {"ID", 26},
        {"FZ", 348},
        {"FY", 348},
        {"FX", 1015},
        {"BI", 320},
        {"HQ", 1031},
        {"GK", 302},
        {"GI", 1001},
        {"AJ", 199},
        {"AL", 328},
        {"AA", 194},
        {"BN", 217},
        {"AH", 348},
        {"FN", 226},
        {"FM", 1009},
        {"HH", 1009},
        {"FK", 364},
        {"JO", 346},
        {"IS", 1003},
        {"GP", 1001},
        {"FG", 311},
        {"FP", 1031},
        {"FJ", 1020},
        {"FD", 199},
        {"JR", 335},
        {"AK", 194},
        {"FA", 1031},
        {"EZ", 302},
        {"BH", 1003},
        {"FH", 1001},
        {"EW", 320},
        {"EV", 302},
        {"HO", 199},
        {"AP", 304},
        {"AG", 333},
        {"GS", 1001},
        {"AI", 106},
        {"EP", 348},
        {"JX", 1016},
        {"EN", 348},
        {"EM", 1004},
        {"EL", 1009},
        {"EK", 1001},
        {"EJ", 194},
        {"AQ", 24},
        {"BW", 1003},
        {"AN", 334},
        {"CV", 1004},
        {"FV", 321},
        {"ER", 1001},
        {"GC", 1004},
        {"EB", 199},
        {"EA", 45},
        {"DZ", 346},
        {"FU", 1001},
        {"AM", 325},
        {"DW", 1020},
        {"DV", 1008},
        {"DU", 1002},
        {"DT", 1002},
        {"DS", 311},
        {"HB", 1020},
        {"DQ", 194},
        {"FC", 1001},
        {"DO", 199},
        {"JP", 194},
        {"DM", 1001},
        {"DL", 1003},
        {"DK", 1004},
        {"DJ", 1001},
        {"DI", 199},
        {"DH", 341},
        {"DG", 1032},
        {"DF", 1001},
        {"DE", 359},
        {"FF", 1001},
        {"DC", 1003},
        {"DB", 1020},
        {"DA", 363},
        {"CZ", 106},
        {"CY", 1003},
        {"CX", 302},
        {"CW", 302},
        {"HF", 327},
        {"CU", 1019},
        {"CT", 194},
        {"CS", 335},
        {"CR", 1003},
        {"HZ", 194},
        {"CP", 327},
        {"CO", 1017},
        {"CN", 24},
        {"CM", 194},
        {"CL", 26},
        {"CK", 26},
        {"CJ", 226},
        {"CI", 1002},
        {"DD", 194},
        {"CG", 353},
        {"CF", 1019},
        {"GF", 1020},
        {"CD", 1001},
        {"CC", 1004},
        {"CB", 1020},
        {"CA", 1020},
        {"HW", 1016},
        {"BY", 353},
        {"BX", 307},
        {"FT", 302},
        {"KI", 364},
        {"BU", 349},
        {"BT", 310},
        {"BS", 217},
        {"BR", 325},
        {"FW", 354},
        {"BP", 1001},
        {"IL", 217},
        {"BQ", 194},
        {"BM", 354},
        {"JU", 1001},
        {"BK", 24},
        {"BJ", 1003},
        {"EE", 26},
        {"EF", 1003},
        {"BG", 354},
        {"BF", 26},
        {"BE", 1002},
        {"JT", 348},
        {"BC", 38},
        {"BB", 301},
        {"BA", 336},
        {"AZ", 363},
        {"AY", 26},
        {"AX", 341},
        {"AW", 21},
        {"AV", 1008},
        {"AU", 26},
        {"EC", 1013},
        {"AS", 327},
        {"HR", 354},
        {"IC", 363},
        {"HX", 199},
        {"BL", 199},
        {"AC", 354},
        {"FE", 141},
        {"FO", 208},
        {"GA", 194},
        {"GD", 1004},
        {"JQ", 1020},
        {"GR", 1003},
        {"HI", 1010},
        {"IA", 306},
        {"IG", 141},
        {"IJ", 1001},
        {"IM", 199},
        {"IN", 107},
        {"IO", 1003},
        {"IP", 217},
        {"IQ", 45},
        {"IR", 363},
        {"IT", 1001},
        {"CH", 5},
        {"IV", 26},
        {"EY", 338},
        {"JD", 1020},
        {"JG", 1001},
        {"IY", 1013},
        {"GY", 9},
        {"IX", 332},
        {"DN", 1020},
        {"JF", 194},
        {"BO", 1016},
        {"JA", 356},
        {"IZ", 1001},
        {"JB", 217},
        {"JC", 1003},
        {"JH", 1003},
        {"JK", 320},
        {"JL", 1005},
        {"JJ", 363},
        {"EU", 107},
        {"JM", 302},
        {"JN", 1004},
        {"GZ", 151},
        {"DR", 1003},
        {"EO", 311},
        {"GL", 133},
        {"IW", 1029},
        {"JS", 23},
        {"BD", 1009},
        {"HY", 1004},
        {"EX", 311},
        {"IE", 1001},
        {"JW", 199},
        {"CE", 1001},
        {"GE", 314},
        {"JY", 1001},
        {"KB", 342},
        {"KA", 226},
        {"JZ", 21},
        {"KC", 38},
        {"KD", 141},
        {"EQ", 1013},
        {"AB", 105},
        {"HV", 301},
        {"KE", 302},
        {"FR", 350},
        {"FS", 5},
        {"II", 363},
        {"KF", 1003},
        {"KJ", 1004},
        {"KH", 199},
        {"KG", 345},
        {"KK", 199},
        {"BV", 23},
        {"KL", 348},
        {"KM", 331},
        {"KO", 141},
        {"KN", 1012},
        {"KP", 327},
        {"KQ", 135},
        {"KR", 336},
        {"KS", 306},
        {"KT", 1002},
        {"KU", 1014},
        {"KV", 313},
        {"KZ", 335}
    };
    Dictionary<string, int> partShape = new Dictionary<string, int>()
    {
        {"AA", 3},
        {"AB", 0},
        {"AC", 0},
        {"AD", 0},
        {"AE", 0},
        {"AF", 0},
        {"AG", 3},
        {"AH", 0},
        {"AI", 0},
        {"AJ", 0},
        {"AK", 0},
        {"AL", 0},
        {"AM", 0},
        {"AN", 0},
        {"AO", 0},
        {"AP", 0},
        {"AQ", 0},
        {"AR", 0},
        {"AS", 0},
        {"AT", 0},
        {"AU", 0},
        {"AV", 0},
        {"AW", 0},
        {"AX", 0},
        {"AY", 0},
        {"AZ", 3},
        {"BA", 0},
        {"BB", 0},
        {"BC", 0},
        {"BD", 0},
        {"BE", 0},
        {"BF", 0},
        {"BG", 0},
        {"BH", 4},
        {"BI", 3},
        {"BJ", 4},
        {"BK", 0},
        {"BL", 0},
        {"BM", 0},
        {"BN", 0},
        {"BO", 0},
        {"BP", 0},
        {"BQ", 1},
        {"BR", 0},
        {"BS", 0},
        {"BT", 0},
        {"BU", 0},
        {"BV", 3},
        {"BW", 0},
        {"BX", 0},
        {"BY", 0},
        {"BZ", 0},
        {"CA", 0},
        {"CB", 0},
        {"CC", 3},
        {"CD", 0},
        {"CE", 0},
        {"CF", 0},
        {"CG", 3},
        {"CH", 0},
        {"CI", 0},
        {"CJ", 6},
        {"CK", 0},
        {"CL", 6},
        {"CM", 6},
        {"CN", 0},
        {"CO", 0},
        {"CP", 0},
        {"CQ", 0},
        {"CR", 0},
        {"CS", 0},
        {"CT", 0},
        {"CU", 0},
        {"CV", 0},
        {"CW", 0},
        {"CX", 0},
        {"CY", 4},
        {"CZ", 0},
        {"DA", 0},
        {"DB", 0},
        {"DC", 3},
        {"DD", 4},
        {"DE", 0},
        {"DF", 0},
        {"DG", 0},
        {"DH", 5},
        {"DI", 0},
        {"DJ", 0},
        {"DK", 0},
        {"DL", 0},
        {"DM", 0},
        {"DN", 3},
        {"DO", 3},
        {"DP", 0},
        {"DQ", 0},
        {"DR", 0},
        {"DS", 0},
        {"DT", 0},
        {"DU", 0},
        {"DV", 0},
        {"DW", 0},
        {"DX", 0},
        {"DY", 0},
        {"DZ", 0},
        {"EA", 0},
        {"EB", 0},
        {"EC", 0},
        {"ED", 0},
        {"EE", 6},
        {"EF", 0},
        {"EG", 0},
        {"EH", 3},
        {"EI", 0},
        {"EJ", 0},
        {"EK", 0},
        {"EL", 0},
        {"EM", 0},
        {"EN", 1},
        {"EO", 0},
        {"EP", 4},
        {"EQ", 0},
        {"ER", 0},
        {"ES", 0},
        {"ET", 0},
        {"EU", 0},
        {"EV", 0},
        {"EW", 0},
        {"EX", 0},
        {"EY", 0},
        {"EZ", 0},
        {"FA", 0},
        {"FB", 0},
        {"FC", 0},
        {"FD", 5},
        {"FE", 0},
        {"FF", 0},
        {"FG", 5},
        {"FH", 0},
        {"FI", 0},
        {"FJ", 0},
        {"FK", 3},
        {"FL", 0},
        {"FM", 0},
        {"FN", 0},
        {"FO", 0},
        {"FP", 3},
        {"FQ", 0},
        {"FR", 0},
        {"FS", 0},
        {"FT", 0},
        {"FU", 0},
        {"FV", 0},
        {"FW", 0},
        {"FX", 5},
        {"FY", 5},
        {"FZ", 5},
        {"GA", 0},
        {"GB", 3},
        {"GC", 3},
        {"GD", 5},
        {"GE", 0},
        {"GF", 0},
        {"GG", 0},
        {"GH", 0},
        {"GI", 0},
        {"GJ", 0},
        {"GK", 4},
        {"GL", 0},
        {"GM", 0},
        {"GN", 0},
        {"GO", 3},
        {"GP", 0},
        {"GQ", 0},
        {"GR", 4},
        {"GS", 0},
        {"GT", 0},
        {"GU", 2},
        {"GV", 0},
        {"GW", 0},
        {"GX", 0},
        {"GY", 0},
        {"GZ", 0},
        {"HA", 0},
        {"HB", 0},
        {"HC", 0},
        {"HD", 0},
        {"HE", 0},
        {"HF", 3},
        {"HG", 0},
        {"HH", 4},
        {"HI", 0},
        {"HJ", 0},
        {"HK", 0},
        {"HL", 0},
        {"HM", 0},
        {"HN", 0},
        {"HO", 0},
        {"HP", 0},
        {"HQ", 0},
        {"HR", 0},
        {"HS", 0},
        {"HT", 0},
        {"HU", 0},
        {"HV", 3},
        {"HW", 4},
        {"HX", 0},
        {"HY", 0},
        {"HZ", 3},
        {"IA", 0},
        {"IB", 0},
        {"IC", 1},
        {"ID", 0},
        {"IE", 0},
        {"IF", 0},
        {"IG", 0},
        {"IH", 0},
        {"II", 0},
        {"IJ", 0},
        {"IK", 0},
        {"IL", 0},
        {"IM", 5},
        {"IN", 5},
        {"IO", 0},
        {"IP", 0},
        {"IQ", 0},
        {"IR", 0},
        {"IS", 0},
        {"IT", 5},
        {"IU", 4},
        {"IV", 0},
        {"IW", 0},
        {"IX", 3},
        {"IY", 4},
        {"IZ", 0},
        {"JA", 0},
        {"JB", 0},
        {"JC", 0},
        {"JD", 0},
        {"JE", 0},
        {"JF", 0},
        {"JG", 0},
        {"JH", 0},
        {"JI", 0},
        {"JJ", 0},
        {"JK", 3},
        {"JL", 3},
        {"JM", 0},
        {"JN", 0},
        {"JO", 0},
        {"JP", 0},
        {"JQ", 0},
        {"JR", 0},
        {"JS", 0},
        {"JT", 0},
        {"JU", 0},
        {"JV", 0},
        {"JW", 0},
        {"JX", 0},
        {"JY", 0},
        {"JZ", 0},
        {"KA", 0},
        {"KB", 0},
        {"KC", 5},
        {"KD", 0},
        {"KE", 0},
        {"KF", 0},
        {"KG", 0},
        {"KH", 0},
        {"KI", 6},
        {"KJ", 0},
        {"KK", 0},
        {"KL", 0},
        {"KM", 0},
        {"KN", 0},
        {"KO", 0},
        {"KP", 3},
        {"KQ", 0},
        {"KR", 0},
        {"KS", 3},
        {"KT", 0},
        {"KU", 0},
        {"KV", 0},
        {"KW", 5},
        {"KX", 0},
        {"KY", 0},
        {"KZ", 0}
    };

    public List<GameObject> shapes;
    public Material material;
    public Material outlineMaterial;

    public List<blueprintPart> readBlueprint(string blueprint)
    {
        blueprint = blueprint.Replace("~", "||");
        string[] values = blueprint.Split(new char[] { '|' });

        int state = 0;
        Vector3 temp = new Vector3();
        bool skip = false;
        blueprintPart blueprintPart = new blueprintPart();
        blueprintList.Clear();
        for (int i = 0; i < values.Length; i++)
        {
            skip = false;
            //Position and partCode
            if (state == 0)
            {
                if (i != values.Length - 1)
                {
                    if (values[i].Length < 3)
                    {
                        blueprintPart.partCode = "IL";
                        blueprintPart.name = names["IL"];
                        blueprintPart.position.x = 0;
                    }
                    else
                    {
                        blueprintPart.partCode = values[i].Substring(0, 2);
                        if (names.ContainsKey(blueprintPart.partCode))
                        {
                            blueprintPart.name = names[blueprintPart.partCode];
                        }
                        else
                        {
                            blueprintPart.partCode = "IL";
                            blueprintPart.name = names["IL"];
                        }

                        float parseValue;
                        float.TryParse(decode(values[i].Substring(2)), out parseValue);
                        blueprintPart.position.x = parseValue;
                    }
                }
            }

            if (state == 1)
            {
                float parseValue;
                float.TryParse(decode(values[i]), out parseValue);
                blueprintPart.position.y = parseValue;
            }

            if (state == 2)
            {
                float parseValue;
                float.TryParse(decode(values[i]), out parseValue);
                blueprintPart.position.z = parseValue;
            }

            //Rotation euler
            if (state == 3)
            {
                if (values[i] == "")
                {
                    skip = true;
                    blueprintPart.rotation = Vector3.zero;
                }
                else
                {
                    float parseValue;
                    float.TryParse(decode(values[i]), out parseValue);
                    temp.x = parseValue;
                }
            }

            if (state == 4)
            {
                float parseValue;
                float.TryParse(decode(values[i]), out parseValue);
                temp.y = parseValue;
            }

            if (state == 5)
            {
                float parseValue;
                float.TryParse(decode(values[i]), out parseValue);
                temp.z = parseValue;
                blueprintPart.rotation = temp;
            }

            //Scale of the part
            if (state == 6)
            {
                if (values[i] == "")
                {
                    skip = true;
                    if (defaultSizes.ContainsKey(blueprintPart.partCode))
                    {
                        blueprintPart.scale = defaultSizes[blueprintPart.partCode];
                    }
                }
                else
                {
                    float parseValue;
                    float.TryParse(decode(values[i]), out parseValue);
                    temp.x = parseValue;
                }
            }

            if (state == 7)
            {
                float parseValue;
                float.TryParse(decode(values[i]), out parseValue);
                temp.y = parseValue;
            }

            if (state == 8)
            {
                float parseValue;
                float.TryParse(decode(values[i]), out parseValue);
                temp.z = parseValue;
                blueprintPart.scale = temp;
            }

            //Colour
            if (state == 9)
            {
                if (values[i] == "")
                {
                    if (defaultColours.ContainsKey(blueprintPart.partCode))
                    {
                        if (colours.ContainsKey(defaultColours[blueprintPart.partCode]))
                        {
                            blueprintPart.colour = colours[defaultColours[blueprintPart.partCode]];
                        }
                    }
                }
                else
                {
                    int parseValue;
                    int.TryParse(decode(values[i]), out parseValue);
                    if (colours.ContainsKey(parseValue))
                    {
                        blueprintPart.colour = colours[parseValue];
                    }
                }
            }

            //Add Part
            if (state == 10)
            {
                state = -1;
                blueprintList.Add(blueprintPart);
                temp = Vector3.zero;
                blueprintPart = new blueprintPart();
            }

            if (skip)
            {
                state += 3;
            }
            else
            {
                state++;
            }
        }
        return blueprintList;
    }

    public int getPartCount(string blueprint)
    {
        blueprint = blueprint.Replace("~", "||");
        string[] values = blueprint.Split(new char[] { '|' });

        int state = 0;
        bool skip = false;
        int partCount = 0;
        for (int i = 0; i < values.Length; i++)
        {
            skip = false;
            //Position and partCode
            if (state == 0)
            {
                if (i != values.Length - 1)
                {
                    partCount++;
                }
            }

            //Rotation euler
            if (state == 3)
            {
                if (values[i] == "")
                {
                    skip = true;
                }
            }

            //Scale of the part
            if (state == 6)
            {
                if (values[i] == "")
                {
                    skip = true;
                }
            }

            //Add Part
            if (state == 10)
            {
                state = -1;
            }

            if (skip)
            {
                state += 3;
            }
            else
            {
                state++;
            }
        }
        return partCount;
    }

    public string decode(string input)
    {
        string output = input;
        output = output.Replace("A", "9999999");
        output = output.Replace("B", "999999");
        output = output.Replace("C", "99999");
        output = output.Replace("D", "9999");
        output = output.Replace("E", "999");
        output = output.Replace("F", "99");
        output = output.Replace("G", "0000000");
        output = output.Replace("H", "000000");
        output = output.Replace("I", "00000");
        output = output.Replace("J", "0000");
        output = output.Replace("K", "000");
        output = output.Replace("L", "00");
        output = output.Replace("M", "-90");
        output = output.Replace("N", "90");
        output = output.Replace("O", "-180");
        output = output.Replace("P", "180");

        return output;
    }

    public void buildBlueprint(string blueprint, Transform parent)
    {
        buildBlueprint(readBlueprint(blueprint), parent);
    }

    public void buildBlueprint(List<blueprintPart> blueprint, Transform parent)
    {
        parent.localScale = new Vector3(1, 1, 1);
        for (int i = 0; i < blueprint.Count; i++)
        {
            GameObject part = Instantiate(shapes[partShape[blueprint[i].partCode]], blueprint[i].position, Quaternion.identity, parent);
            part.transform.rotation *= Quaternion.Euler(blueprint[i].rotation.x, 0f, 0f);
            part.transform.rotation *= Quaternion.Euler(0f, blueprint[i].rotation.y, 0f);
            part.transform.rotation *= Quaternion.Euler(0f, 0f, blueprint[i].rotation.z);
            part.name = blueprint[i].name;

            part.transform.localScale = blueprint[i].scale;
            Material newMaterial = new Material(material);
            newMaterial.color = blueprint[i].colour;
            part.GetComponent<Renderer>().material = newMaterial;
        }
        parent.localScale = new Vector3(-1, 1, 1);
    }

    public Transform buildAndGetCamera(string blueprint, Transform parent)
    {
        return buildAndGetCamera(readBlueprint(blueprint), parent);
    }

    public Transform buildAndGetCamera(List<blueprintPart> blueprint, Transform parent)
    {
        parent.localScale = new Vector3(1, 1, 1);
        Vector3 maxPos = Vector3.negativeInfinity;
        Vector3 minPos = Vector3.positiveInfinity;
        for (int i = 0; i < blueprint.Count; i++)
        {

            GameObject part = Instantiate(shapes[partShape[blueprint[i].partCode]], blueprint[i].position, Quaternion.identity, parent);
            part.transform.rotation *= Quaternion.Euler(blueprint[i].rotation.x, 0f, 0f);
            part.transform.rotation *= Quaternion.Euler(0f, blueprint[i].rotation.y, 0f);
            part.transform.rotation *= Quaternion.Euler(0f, 0f, blueprint[i].rotation.z);
            part.name = blueprint[i].name;

            part.transform.localScale = blueprint[i].scale;
            Material newMaterial = new Material(material);
            newMaterial.color = blueprint[i].colour;
            var renderer = part.GetComponent<Renderer>();
            renderer.material = newMaterial;

            var bounds = renderer.bounds;
            if (bounds.center.x + bounds.extents.x > maxPos.x)
            {
                maxPos.x = bounds.center.x + bounds.extents.x;
            }
            if (bounds.center.y + bounds.extents.y > maxPos.y)
            {
                maxPos.y = bounds.center.y + bounds.extents.y;
            }
            if (bounds.center.z + bounds.extents.z > maxPos.z)
            {
                maxPos.z = bounds.center.z + bounds.extents.z;
            }

            if (bounds.center.x - bounds.extents.x < minPos.x)
            {
                minPos.x = bounds.center.x - bounds.extents.x;
            }
            if (bounds.center.y - bounds.extents.y < minPos.y)
            {
                minPos.y = bounds.center.y - bounds.extents.y;
            }
            if (bounds.center.z - bounds.extents.z < minPos.z)
            {
                minPos.z = bounds.center.z - bounds.extents.z;
            }
        }
        parent.localScale = new Vector3(-1, 1, 1);

        float temp = maxPos.x * -1f;
        maxPos.x = minPos.x * -1f;
        minPos.x = temp;

        GameObject empty = new GameObject();
        Transform cameraTransform = empty.transform;
        Vector3 centre = new Vector3((maxPos.x + minPos.x) / 2f, (maxPos.y + minPos.y) / 2f, (maxPos.z + minPos.z) / 2f);
        Vector3 position = new Vector3(maxPos.x, maxPos.y, minPos.z);
        position = position + ((position - centre) * 0.5f);
        if (!float.IsNaN(position.x) && !float.IsNaN(position.y) && !float.IsNaN(position.z))
        {
            cameraTransform.position = position;
        }
        cameraTransform.LookAt(centre);
        Destroy(empty.gameObject);

        return cameraTransform;
    }

    public returnInfo buildRuntime(string blueprint, Transform parent)
    {
        return buildRuntime(readBlueprint(blueprint), parent);
    }

    public returnInfo buildRuntime(List<blueprintPart> blueprint, Transform parent)
    {
        parent.localScale = new Vector3(1, 1, 1);
        Vector3 maxPos = Vector3.negativeInfinity;
        Vector3 minPos = Vector3.positiveInfinity;
        for (int i = 0; i < blueprint.Count; i++)
        {

            GameObject part = Instantiate(shapes[partShape[blueprint[i].partCode]], blueprint[i].position, Quaternion.identity, parent);
            part.transform.rotation *= Quaternion.Euler(blueprint[i].rotation.x, 0f, 0f);
            part.transform.rotation *= Quaternion.Euler(0f, blueprint[i].rotation.y, 0f);
            part.transform.rotation *= Quaternion.Euler(0f, 0f, blueprint[i].rotation.z);
            part.name = blueprint[i].name;

            blueprint[i].scale.x *= -1f;
            part.transform.localScale = blueprint[i].scale;
            Material newMaterial = new Material(material);
            newMaterial.color = blueprint[i].colour;
            var renderer = part.GetComponent<Renderer>();
            renderer.material = newMaterial;

            var bounds = renderer.bounds;
            if (bounds.center.x + bounds.extents.x > maxPos.x)
            {
                maxPos.x = bounds.center.x + bounds.extents.x;
            }
            if (bounds.center.y + bounds.extents.y > maxPos.y)
            {
                maxPos.y = bounds.center.y + bounds.extents.y;
            }
            if (bounds.center.z + bounds.extents.z > maxPos.z)
            {
                maxPos.z = bounds.center.z + bounds.extents.z;
            }

            if (bounds.center.x - bounds.extents.x < minPos.x)
            {
                minPos.x = bounds.center.x - bounds.extents.x;
            }
            if (bounds.center.y - bounds.extents.y < minPos.y)
            {
                minPos.y = bounds.center.y - bounds.extents.y;
            }
            if (bounds.center.z - bounds.extents.z < minPos.z)
            {
                minPos.z = bounds.center.z - bounds.extents.z;
            }
        }
        parent.localScale = new Vector3(-1, 1, 1);

        float temp = maxPos.x * -1f;
        maxPos.x = minPos.x * -1f;
        minPos.x = temp;

        GameObject empty = new GameObject();
        Transform cameraTransform = empty.transform;
        Vector3 centre = new Vector3((maxPos.x + minPos.x) / 2f, (maxPos.y + minPos.y) / 2f, (maxPos.z + minPos.z) / 2f);
        Vector3 position = new Vector3(maxPos.x, maxPos.y, minPos.z);
        position = position + ((position - centre) * 0.5f);
        if (!float.IsNaN(position.x) && !float.IsNaN(position.y) && !float.IsNaN(position.z))
        {
            cameraTransform.position = position;
        }
        cameraTransform.LookAt(centre);
        Destroy(empty.gameObject);

        returnInfo returnInfo = new returnInfo();
        returnInfo.cameraTransform = cameraTransform;
        returnInfo.max = maxPos;
        returnInfo.min = minPos;
        returnInfo.centre = centre;

        return returnInfo;
    }



    public void buildOutline(string blueprint, Transform parent, Vector3 rgbColour)
    {
        buildOutline(readBlueprint(blueprint), parent, rgbColour);
    }

    public void buildOutline(List<blueprintPart> blueprint, Transform parent, Vector3 rgbColour)
    {
        parent.localScale = new Vector3(1, 1, 1);
        for (int i = 0; i < blueprint.Count; i++)
        {
            GameObject part = Instantiate(shapes[partShape[blueprint[i].partCode]], blueprint[i].position, Quaternion.identity, parent);
            part.transform.rotation *= Quaternion.Euler(blueprint[i].rotation.x, 0f, 0f);
            part.transform.rotation *= Quaternion.Euler(0f, blueprint[i].rotation.y, 0f);
            part.transform.rotation *= Quaternion.Euler(0f, 0f, blueprint[i].rotation.z);
            part.name = blueprint[i].name;

            blueprint[i].scale.x *= -1f;
            part.transform.localScale = blueprint[i].scale + new Vector3(-2f, 2f, 2f);
            Material newMaterial = new Material(outlineMaterial);
            newMaterial.color = new Color(rgbColour.x, rgbColour.y, rgbColour.z);
            part.GetComponent<Renderer>().material = newMaterial;

            if (partShape[blueprint[i].partCode] == 1)
            {
                part.transform.position += (part.transform.up * 0.5f) + (part.transform.forward * -0.5f);
                part.transform.localScale = blueprint[i].scale + new Vector3(-2f, 1f, 1f);
            }

            if (partShape[blueprint[i].partCode] == 2)
            {
                part.transform.position += (part.transform.up * 0.5f) + (part.transform.forward * 0.5f) + (part.transform.right * -0.5f);
                part.transform.localScale = blueprint[i].scale + new Vector3(-1f, 1f, 1f);
            }
        }
        parent.localScale = new Vector3(-1, 1, 1);
    }



    public void buildOutlineImage(string blueprint, Transform parent, Vector3 rgbColour)
    {
        buildOutlineImage(readBlueprint(blueprint), parent, rgbColour);
    }

    public void buildOutlineImage(List<blueprintPart> blueprint, Transform parent, Vector3 rgbColour)
    {
        parent.localScale = new Vector3(1, 1, 1);
        for (int i = 0; i < blueprint.Count; i++)
        {
            GameObject part = Instantiate(shapes[partShape[blueprint[i].partCode]], blueprint[i].position, Quaternion.identity, parent);
            part.transform.rotation *= Quaternion.Euler(blueprint[i].rotation.x, 0f, 0f);
            part.transform.rotation *= Quaternion.Euler(0f, blueprint[i].rotation.y, 0f);
            part.transform.rotation *= Quaternion.Euler(0f, 0f, blueprint[i].rotation.z);
            part.name = blueprint[i].name;

            part.transform.localScale = blueprint[i].scale + new Vector3(2f, 2f, 2f);
            Material newMaterial = new Material(outlineMaterial);
            newMaterial.color = new Color(rgbColour.x, rgbColour.y, rgbColour.z);
            part.GetComponent<Renderer>().material = newMaterial;

            if (partShape[blueprint[i].partCode] == 1)
            {
                part.transform.position += (part.transform.up * 0.5f) + (part.transform.forward * -0.5f);
                part.transform.localScale = blueprint[i].scale + new Vector3(2f, 1f, 1f);
            }

            if (partShape[blueprint[i].partCode] == 2)
            {
                part.transform.position += (part.transform.up * 0.5f) + (part.transform.forward * 0.5f) + (part.transform.right * -0.5f);
                part.transform.localScale = blueprint[i].scale + new Vector3(1f, 1f, 1f);
            }
        }
        parent.localScale = new Vector3(-1, 1, 1);
    }
}


[System.Serializable]
public class blueprintPart
{
    public string name;
    public string partCode;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
    public Color colour;
}

public class returnInfo
{
    public Transform cameraTransform;
    public Vector3 min;
    public Vector3 max;
    public Vector3 centre;
}
