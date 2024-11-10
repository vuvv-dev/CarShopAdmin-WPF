﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domains.Entities;

namespace Domains.Repostiories;

public interface ICarStatusRepository
{
    List<CarStatus> GetAllCarStatuses();
}