using System;
using System.Collections.Generic;
using VSNext.Mongo.Repository;

namespace VSNext.Mongo.Entities
{
    [CollectionName("Tenant")]
    public class Tenant : Entity
    {       
        public string AccountName { get; set; }
    }
}
