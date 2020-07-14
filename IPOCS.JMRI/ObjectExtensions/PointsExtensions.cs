﻿using IPOCS.JMRI.Translators;
using IPOCS.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IPOCS.JMRI.ObjectExtensions
{
  public static class PointsExtensions
  {
    public static BaseTranslator GetTranslator(this Packet basePkt)
    {
      var allTranslators = from lAssembly in AppDomain.CurrentDomain.GetAssemblies()
                           from lType in lAssembly.GetTypes()
                           where typeof(BaseTranslator).IsAssignableFrom(lType) && !lType.IsAbstract
                           select lType;
      foreach (var translator in allTranslators)
      {
        var baseTranslator = Activator.CreateInstance(translator) as BaseTranslator;
        if (baseTranslator.SupportedPackets().Contains(basePkt.GetType()))
        {
          Console.WriteLine($"Found translator: {baseTranslator.GetType().Name}");
          return baseTranslator;
        }
      }
      Console.WriteLine($"onMessage: Unexpected package type ({ basePkt.GetType().FullName })");
      return null;
    }
  }
}
