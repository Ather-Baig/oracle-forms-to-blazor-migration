namespace FormsMigrationApp.Models
{
    public class QualitySetup
    {
        public string QualityCode { get; set; } = "";      // QUALITYCODE (PK, VARCHAR2)
        public string? WarpCount { get; set; }             // WARPCOUNT
        public string? WeftCount { get; set; }              // WEFTCOUNT
        public decimal PicksPerInch { get; set; }           // PICKSPERINCH (not null)
        public decimal? EndsPerInch { get; set; }            // ENDSPERINCH
        public string? Width { get; set; }                  // WIDTH
        public string? Weave { get; set; }                  // WEAVE
        public string? Twill { get; set; }                  // TWILL
        public string? Colour { get; set; }                 // COLOUR
        public string? Construction { get; set; }            // CONSTRUCTION
        public string? BrandName { get; set; }               // BRANDNAME
        public string? FType { get; set; }                   // F_TYPE
        public decimal? Oz { get; set; }                      // OZ
        public int Panel { get; set; } = 1;                   // PANEL (default 1)
        public int Insertion { get; set; } = 1;               // INSERTION (default 1)
        public decimal? ProdPicksPerInch { get; set; }        // PRODPICKSPERINCH
        public decimal? StdEff { get; set; }                  // STDEFF
        public string? LenoColor { get; set; }                // LENOCOLOR
        public decimal? Contraction { get; set; } = 8;         // CONTRACTION (default 8)
        public string? Remarks { get; set; }                  // REMARKS
        public string Running { get; set; } = "Y";            // RUNNING (default 'Y', not null)
        public string? UserName { get; set; }                 // USER_NAME (default USER)
        public DateTime? DateOfEntry { get; set; }             // DATTE_OF_ENTRY (default SYSDATE)
        public decimal? WpCount { get; set; }                  // WP_COUNT
        public string? WpSplice { get; set; }                  // WP_SPLICE
        public string? WpYarnType { get; set; }                // WP_YARN_TYPE
        public decimal? WfCount { get; set; }                  // WF_COUNT
        public string? WfSplice { get; set; }                  // WF_SPLICE
        public string? WfYarnType { get; set; }                // WF_YARN_TYPE
        public decimal? Width1 { get; set; }                   // WIDTH1
        public decimal? Width2 { get; set; }                   // WIDTH2
        public decimal? Width3 { get; set; }                   // WIDTH3
        public decimal? ActualWidth { get; set; }              // ACTUAL_WIDTH
        public char? WpCountSystem { get; set; }               // WP_COUNT_SYSTEM
        public char? WfCountSystem { get; set; }               // WF_COUNT_SYSTEM
        public string? PlanEff { get; set; } = "90";           // PLAN_EFF (default '90')
        public decimal? StdRpm { get; set; } = 850;            // STDRPM (default 850)
        public string? Selvedge { get; set; }                  // SELVEDGE
        public string? WpSource { get; set; }                  // WP_SOURCE
        public string? WfSource { get; set; }                  // WF_SOURCE
        public decimal? RatePerPpi { get; set; }               // RATE_PER_PPI
        public string? Reed { get; set; }                      // REED
        public string? ContractNo { get; set; }                // CONTRACTNO
        public decimal? Ppi { get; set; }                      // PPI
    }
}
