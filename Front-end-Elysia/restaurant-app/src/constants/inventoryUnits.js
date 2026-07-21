export const UNIDADES_MEDIDA = [
  {
    grupo: "Unidades",
    opciones: [
      { valor: "ud", etiqueta: "Unidad (ud)" },
      { valor: "pz", etiqueta: "Pieza (pz)" },
      { valor: "par", etiqueta: "Par" },
      { valor: "doc", etiqueta: "Docena (12)" },
      { valor: "mdoc", etiqueta: "Media docena (6)" },
      { valor: "cen", etiqueta: "Centena (100)" },
    ],
  },

  {
    grupo: "Empaque",
    opciones: [
      { valor: "paq", etiqueta: "Paquete" },
      { valor: "caja", etiqueta: "Caja" },
      { valor: "saco", etiqueta: "Saco" },
      { valor: "fund", etiqueta: "Funda" },
      { valor: "bols", etiqueta: "Bolsa" },
      { valor: "band", etiqueta: "Bandeja" },
      { valor: "cube", etiqueta: "Cubeta" },
      { valor: "roll", etiqueta: "Rollo" },
      { valor: "lata", etiqueta: "Lata" },
      { valor: "bot", etiqueta: "Botella" },
      { valor: "fras", etiqueta: "Frasco" },
      { valor: "tarr", etiqueta: "Tarro" },
    ],
  },

  {
    grupo: "Peso",
    opciones: [
      { valor: "mg", etiqueta: "Miligramo (mg)" },
      { valor: "g", etiqueta: "Gramo (g)" },
      { valor: "kg", etiqueta: "Kilogramo (kg)" },
      { valor: "oz", etiqueta: "Onza (oz)" },
      { valor: "lb", etiqueta: "Libra (lb)" },
      { valor: "qq", etiqueta: "Quintal (qq)" },
      { valor: "t", etiqueta: "Tonelada (t)" },
    ],
  },

  {
    grupo: "Volumen",
    opciones: [
      { valor: "ml", etiqueta: "Mililitro (ml)" },
      { valor: "cl", etiqueta: "Centilitro (cl)" },
      { valor: "l", etiqueta: "Litro (L)" },
      { valor: "gal", etiqueta: "Galón (gal)" },
      { valor: "qt", etiqueta: "Cuarto (qt)" },
      { valor: "pt", etiqueta: "Pinta (pt)" },
      { valor: "floz", etiqueta: "Onza líquida (fl oz)" },
      { valor: "taza", etiqueta: "Taza" },
      { valor: "mtaz", etiqueta: "Media taza" },
      { valor: "cda", etiqueta: "Cucharada (cda)" },
      { valor: "cdta", etiqueta: "Cucharadita (cdta)" },
      { valor: "gota", etiqueta: "Gota" },
    ],
  },

  {
    grupo: "Longitud",
    opciones: [
      { valor: "mm", etiqueta: "Milímetro (mm)" },
      { valor: "cm", etiqueta: "Centímetro (cm)" },
      { valor: "m", etiqueta: "Metro (m)" },
    ],
  },

  {
    grupo: "Gastronomía",
    opciones: [
      { valor: "porc", etiqueta: "Porción" },
      { valor: "rac", etiqueta: "Ración" },
      { valor: "serv", etiqueta: "Servicio" },
      { valor: "pizc", etiqueta: "Pizca" },
      { valor: "manj", etiqueta: "Manojo" },
      { valor: "dien", etiqueta: "Diente" },
      { valor: "roda", etiqueta: "Rodaja" },
      { valor: "reba", etiqueta: "Rebanada" },
      { valor: "file", etiqueta: "Filete" },
    ],
  },
];

export const VALORES_UNIDADES_MEDIDA = UNIDADES_MEDIDA.flatMap(
  (grupo) => grupo.opciones.map((opcion) => opcion.valor)
);