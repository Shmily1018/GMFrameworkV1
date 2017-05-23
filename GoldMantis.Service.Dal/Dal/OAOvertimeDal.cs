using GoldMantis.Entity;

namespace GoldMantis.Service.Dal.Dal
{
    public class OAOvertimeDal:RepositoryTable<OAOvertime>
    {
        public override string KeyID
        {
            get { return "ID"; }
            set { base.KeyID = value; }
        }
    }
}