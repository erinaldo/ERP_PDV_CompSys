using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MataFome.DB.Enums
{
    public enum UF
    {
        [Display(Description = "Acre ", ShortName = "AC")]
        AC,
        [Display(Description = "Alagoas", ShortName = "AL")]
        AL,
        [Display(Description = "Amazonas", ShortName = "AM")]
        AM,
        [Display(Description = "Amapá", ShortName = "AP")]
        AP,
        [Display(Description = "Bahia", ShortName = "BA")]
        BA,
        [Display(Description = "Ceará", ShortName = "CE")]
        CE,
        [Display(Description = "Distrito Federal", ShortName = "DF")]
        DF,
        [Display(Description = "Espírito Santo", ShortName = "ES")]
        ES,
        [Display(Description = "Exterior", ShortName = "EX")]
        EX,
        [Display(Description = "Goiás", ShortName = "GO")]
        GO,
        [Display(Description = "Maranhão", ShortName = "MA")]
        MA,
        [Display(Description = "Minas Gerais", ShortName = "MG")]
        MG,
        [Display(Description = "Mato Grosso do Sul", ShortName = "MS")]
        MS,
        [Display(Description = "Mato Grosso", ShortName = "MT")]
        MT,
        [Display(Description = "Pará  ", ShortName = "PA")]
        PA,
        [Display(Description = "Paraíba", ShortName = "PB")]
        PB,
        [Display(Description = "Pernambuco", ShortName = "PE")]
        PE,
        [Display(Description = "Piauí", ShortName = "PI")]
        PI,
        [Display(Description = "Paraná", ShortName = "PR")]
        PR,
        [Display(Description = "Rio de Janeiro", ShortName = "RJ")]
        RJ,
        [Display(Description = "Rio Grande do Norte", ShortName = "RN")]
        RN,
        [Display(Description = "Rondônia", ShortName = "RO")]
        RO,
        [Display(Description = "Roraima", ShortName = "RR")]
        RR,
        [Display(Description = "Rio Grande do Sul", ShortName = " RS")]
        RS,
        [Display(Description = "Santa Catarina", ShortName = "SC")]
        SC,
        [Display(Description = "Sergipe", ShortName = "SE")]
        SE,
        [Display(Description = "São Paulo", ShortName = "SP")]
        SP,
        [Display(Description = "Tocantins", ShortName = "TO")]
        TO,
    }
}
