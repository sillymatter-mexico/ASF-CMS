using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using asf.cms.dal;
using asf.cms.model;

namespace asf.cms.bll
{
    public class Recuperacion
    {
        public List<RecuperacionVO> GetList()
        {
            RecuperacionDAL rdal = new RecuperacionDAL();
            return new List<RecuperacionVO>(rdal.GetAllActive());
        }

        }

}
