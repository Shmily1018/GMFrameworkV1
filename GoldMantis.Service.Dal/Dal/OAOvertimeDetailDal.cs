using GoldMantis.Entity;

namespace GoldMantis.Service.Dal.Dal
{
    public class OAOvertimeDetailDal:RepositoryTable<OAOvertimeDetail>
    {
        public override string KeyID
        {
            get { return "ID"; }
            set { base.KeyID = value; }
        }
    }
}