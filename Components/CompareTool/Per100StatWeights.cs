namespace NBADemo.Components.CompareTool
{
    public class Per100StatWeights
    {
        public int PTSPer100 { get; set; } = 15;
        public int FTAPer100 { get; set; } = 8;
        public int FTPer100 { get; set; } = 7;
        public int ORBPer100 { get; set; } = 10;
        public int DRBPer100 { get; set; } = 10;
        public int ASTPer100 { get; set; } = 10;
        public int STLPer100 { get; set; } = 10;
        public int BLKPer100 { get; set; } = 10;
        public int TOVPer100 { get; set; } = 10;
        public int PFPer100 { get; set; } = 10;

        public bool Equals100
        {
            get
            {
                return PTSPer100 + FTAPer100 + FTPer100 + ORBPer100 + DRBPer100 + ASTPer100 + STLPer100 + BLKPer100 + TOVPer100 + PFPer100 == 100;
            }
        }

        public int Total
        {
            get
            {
                return PTSPer100 + FTAPer100 + FTPer100 + ORBPer100 + DRBPer100 + ASTPer100 + STLPer100 + BLKPer100 + TOVPer100 + PFPer100;
            }
        }
    }
}
