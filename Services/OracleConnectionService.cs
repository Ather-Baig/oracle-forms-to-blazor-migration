using Oracle.ManagedDataAccess.Client;
using FormsMigrationApp.Models;

namespace FormsMigrationApp.Services
{
    public class OracleConnectionService
    {
        private readonly string _connectionString;

        public OracleConnectionService(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Equivalent of Forms 6i "Execute Query" — filters by quality code or brand name (partial match)
        public async Task<List<QualitySetup>> QueryQualitySetupsAsync(string? qualityCodeFilter = null, string? brandFilter = null)
        {
            var results = new List<QualitySetup>();

            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            var sql = "SELECT QUALITYCODE, WARPCOUNT, WEFTCOUNT, PICKSPERINCH, ENDSPERINCH, WIDTH, " +
                      "WEAVE, TWILL, COLOUR, CONSTRUCTION, BRANDNAME, F_TYPE, OZ, PANEL, INSERTION, " +
                      "REMARKS, RUNNING, REED, PPI " +
                      "FROM SETUP_QUALITY WHERE 1=1";

            if (!string.IsNullOrWhiteSpace(qualityCodeFilter))
                sql += " AND QUALITYCODE = :qualityCode";

            if (!string.IsNullOrWhiteSpace(brandFilter))
                sql += " AND UPPER(BRANDNAME) LIKE UPPER(:brand)";

            using var cmd = new OracleCommand(sql, conn);

            if (!string.IsNullOrWhiteSpace(qualityCodeFilter))
                cmd.Parameters.Add(new OracleParameter("qualityCode", qualityCodeFilter));

            if (!string.IsNullOrWhiteSpace(brandFilter))
                cmd.Parameters.Add(new OracleParameter("brand", "%" + brandFilter + "%"));

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(new QualitySetup
                {
                    QualityCode = reader.GetString(0),
                    WarpCount = reader.IsDBNull(1) ? null : reader.GetString(1),
                    WeftCount = reader.IsDBNull(2) ? null : reader.GetString(2),
                    PicksPerInch = reader.GetDecimal(3),
                    EndsPerInch = reader.IsDBNull(4) ? null : reader.GetDecimal(4),
                    Width = reader.IsDBNull(5) ? null : reader.GetString(5),
                    Weave = reader.IsDBNull(6) ? null : reader.GetString(6),
                    Twill = reader.IsDBNull(7) ? null : reader.GetString(7),
                    Colour = reader.IsDBNull(8) ? null : reader.GetString(8),
                    Construction = reader.IsDBNull(9) ? null : reader.GetString(9),
                    BrandName = reader.IsDBNull(10) ? null : reader.GetString(10),
                    FType = reader.IsDBNull(11) ? null : reader.GetString(11),
                    Oz = reader.IsDBNull(12) ? null : reader.GetDecimal(12),
                    Panel = reader.IsDBNull(13) ? 1 : reader.GetInt32(13),
                    Insertion = reader.IsDBNull(14) ? 1 : reader.GetInt32(14),
                    Remarks = reader.IsDBNull(15) ? null : reader.GetString(15),
                    Running = reader.IsDBNull(16) ? "Y" : reader.GetString(16),
                    Reed = reader.IsDBNull(17) ? null : reader.GetString(17),
                    Ppi = reader.IsDBNull(18) ? null : reader.GetDecimal(18)
                });
            }

            return results;
        }

        // Equivalent of Forms 6i "Save" — inserts if new, updates if existing (based on QUALITYCODE)
        public async Task SaveQualitySetupAsync(QualitySetup record)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            var existsCmd = new OracleCommand(
                "SELECT COUNT(*) FROM SETUP_QUALITY WHERE QUALITYCODE = :qualityCode", conn);
            existsCmd.Parameters.Add(new OracleParameter("qualityCode", record.QualityCode));
            var count = Convert.ToInt32(await existsCmd.ExecuteScalarAsync());

            OracleCommand cmd;
            if (count > 0)
            {
                cmd = new OracleCommand(@"
                    UPDATE SETUP_QUALITY SET
                        WARPCOUNT = :warpCount, WEFTCOUNT = :weftCount, PICKSPERINCH = :picksPerInch,
                        ENDSPERINCH = :endsPerInch, WIDTH = :width, WEAVE = :weave, TWILL = :twill,
                        COLOUR = :colour, CONSTRUCTION = :construction, BRANDNAME = :brandName,
                        F_TYPE = :fType, OZ = :oz, PANEL = :panel, INSERTION = :insertion,
                        REMARKS = :remarks, RUNNING = :running, REED = :reed, PPI = :ppi
                    WHERE QUALITYCODE = :qualityCode", conn);
            }
            else
            {
                cmd = new OracleCommand(@"
                    INSERT INTO SETUP_QUALITY
                        (QUALITYCODE, WARPCOUNT, WEFTCOUNT, PICKSPERINCH, ENDSPERINCH, WIDTH, WEAVE,
                         TWILL, COLOUR, CONSTRUCTION, BRANDNAME, F_TYPE, OZ, PANEL, INSERTION,
                         REMARKS, RUNNING, REED, PPI)
                    VALUES
                        (:qualityCode, :warpCount, :weftCount, :picksPerInch, :endsPerInch, :width, :weave,
                         :twill, :colour, :construction, :brandName, :fType, :oz, :panel, :insertion,
                         :remarks, :running, :reed, :ppi)", conn);
            }

            cmd.Parameters.Add(new OracleParameter("qualityCode", record.QualityCode));
            cmd.Parameters.Add(new OracleParameter("warpCount", (object?)record.WarpCount ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("weftCount", (object?)record.WeftCount ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("picksPerInch", record.PicksPerInch));
            cmd.Parameters.Add(new OracleParameter("endsPerInch", (object?)record.EndsPerInch ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("width", (object?)record.Width ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("weave", (object?)record.Weave ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("twill", (object?)record.Twill ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("colour", (object?)record.Colour ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("construction", (object?)record.Construction ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("brandName", (object?)record.BrandName ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("fType", (object?)record.FType ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("oz", (object?)record.Oz ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("panel", record.Panel));
            cmd.Parameters.Add(new OracleParameter("insertion", record.Insertion));
            cmd.Parameters.Add(new OracleParameter("remarks", (object?)record.Remarks ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("running", record.Running));
            cmd.Parameters.Add(new OracleParameter("reed", (object?)record.Reed ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("ppi", (object?)record.Ppi ?? DBNull.Value));

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
