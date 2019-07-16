using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NHibernate
{
    public partial interface IQuery
    {
      //  object UniqueResult();

        /// <summary>
        /// Strongly-typed version of <see cref="UniqueResult()"/>.
        /// </summary>
       // int UniqueResult<int>();

        /// <summary>
        /// Execute the update or delete statement.
        /// </summary>
        /// <returns> The number of entities updated or deleted. </returns>
        int ExecuteUpdate();
    }
}