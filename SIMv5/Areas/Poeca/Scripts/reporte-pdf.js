function crearReportePdf(elementos) {
    var pdf = new jsPDF({
        orientation: 'landscape',
        unit: 'px',
        format: [1080, 1660]
    });

    pdf.autoTable({
        body: elementos,
        columns: [
            { header: "Sector", dataKey: "sector" },
            { header: "Medida", dataKey: "medida" },
            { header: "Acción", dataKey: "accion" },
            { header: "Meta", dataKey: "meta" },
            { header: "Cantidad Total", dataKey: "seguimiento" },
            { header: "Fechas", dataKey: "fechas" },
            { header: "Inversión", dataKey: "valoracionEconomica" },
            { header: "Observaciones", dataKey: "observaciones" },
        ]
    })

    var timestamp = new Date()

    let filename = `reporte-seguimientos-${timestamp.getDate()}-${timestamp.getMonth()}-${timestamp.getFullYear()}.pdf`;

    pdf.save(filename);
}