window.localStorageHelper = {
    setItem: function (key, value) {
        localStorage.setItem(key, value);
    },
    getItem: function (key) {
        return localStorage.getItem(key);
    },
    removeItem: function (key) {
        localStorage.removeItem(key);
    },
    clear: function () {
        localStorage.clear();
    },
    // Helper methods for competency data
    getCompetencyValues: function () {
        return localStorage.getItem('slider_values');
    },
    setCompetencyValues: function (values) {
        localStorage.setItem('slider_values', values);
    },
    getCompetencyNotes: function () {
        return localStorage.getItem('slider_notes');
    },
    setCompetencyNotes: function (notes) {
        localStorage.setItem('slider_notes', notes);
    },
    clearCompetencyData: function () {
        localStorage.removeItem('slider_values');
        localStorage.removeItem('slider_notes');
    }
};

// Radar Chart Drawing Function
window.drawRadarChart = function(canvasId, data) {
    const canvas = document.getElementById(canvasId);
    if (!canvas) {
        console.error('Canvas element not found:', canvasId);
        return;
    }
    
    const ctx = canvas.getContext('2d');
    if (!ctx) {
        console.error('Canvas context not available');
        return;
    }

    // Validate input data
    if (!data || !data.categories || !data.values || data.categories.length === 0) {
        console.warn('Invalid or empty chart data provided');
        return;
    }

    if (data.categories.length !== data.values.length) {
        console.error('Categories and values arrays must have the same length');
        return;
    }
    
    const centerX = canvas.width / 2;
    const centerY = canvas.height / 2;
    const radius = Math.min(centerX, centerY) - 80;
    
    // Clear canvas completely
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    
    const categories = data.categories;
    const values = data.values;
    const maxValue = 5; // Dreyfus scale 1-5
    
    // Set default colors if not provided
    const colors = data.colors || {
        line: "#007bff",
        fill: "rgba(0, 123, 255, 0.2)",
        point: "#007bff",
        grid: "#e9ecef"
    };
    
    // Draw grid circles
    ctx.strokeStyle = colors.grid;
    ctx.lineWidth = 1;
    for (let i = 1; i <= maxValue; i++) {
        const gridRadius = (radius / maxValue) * i;
        ctx.beginPath();
        ctx.arc(centerX, centerY, gridRadius, 0, 2 * Math.PI);
        ctx.stroke();
        
        // Draw level labels
        ctx.fillStyle = '#6c757d';
        ctx.font = '12px Arial';
        ctx.textAlign = 'center';
        ctx.textBaseline = 'middle';
        ctx.fillText(i.toString(), centerX, centerY - gridRadius + 5);
    }
    
    // Draw axis lines and labels
    const angleStep = (2 * Math.PI) / categories.length;
    ctx.strokeStyle = colors.grid;
    ctx.lineWidth = 1;
    
    for (let i = 0; i < categories.length; i++) {
        const angle = i * angleStep - Math.PI / 2; // Start from top
        const x = centerX + Math.cos(angle) * radius;
        const y = centerY + Math.sin(angle) * radius;
        
        // Draw axis line
        ctx.beginPath();
        ctx.moveTo(centerX, centerY);
        ctx.lineTo(x, y);
        ctx.stroke();
        
        // Draw category labels with better positioning
        const labelDistance = radius + 40;
        const labelX = centerX + Math.cos(angle) * labelDistance;
        const labelY = centerY + Math.sin(angle) * labelDistance;
        
        ctx.fillStyle = '#212529';
        ctx.font = 'bold 11px Arial';
        ctx.textAlign = 'center';
        ctx.textBaseline = 'middle';
        
        // Wrap long category names
        const category = categories[i];
        const words = category.split(' ');
        if (words.length > 2) {
            const firstLine = words.slice(0, 2).join(' ');
            const secondLine = words.slice(2).join(' ');
            ctx.fillText(firstLine, labelX, labelY - 6);
            ctx.fillText(secondLine, labelX, labelY + 6);
        } else if (words.length === 2) {
            ctx.fillText(words[0], labelX, labelY - 6);
            ctx.fillText(words[1], labelX, labelY + 6);
        } else {
            ctx.fillText(category, labelX, labelY);
        }
    }
    
    // Draw data polygon
    if (values.length > 0) {
        ctx.strokeStyle = colors.line;
        ctx.fillStyle = colors.fill;
        ctx.lineWidth = 3;
        ctx.lineJoin = 'round';
        ctx.lineCap = 'round';
        
        ctx.beginPath();
        for (let i = 0; i < values.length; i++) {
            const angle = i * angleStep - Math.PI / 2;
            const value = Math.max(0, Math.min(maxValue, values[i])); // Clamp between 0 and maxValue
            const distance = (radius / maxValue) * value;
            const x = centerX + Math.cos(angle) * distance;
            const y = centerY + Math.sin(angle) * distance;
            
            if (i === 0) {
                ctx.moveTo(x, y);
            } else {
                ctx.lineTo(x, y);
            }
        }
        ctx.closePath();
        ctx.fill();
        ctx.stroke();
        
        // Draw data points
        ctx.fillStyle = colors.point;
        for (let i = 0; i < values.length; i++) {
            const angle = i * angleStep - Math.PI / 2;
            const value = Math.max(0, Math.min(maxValue, values[i]));
            const distance = (radius / maxValue) * value;
            const x = centerX + Math.cos(angle) * distance;
            const y = centerY + Math.sin(angle) * distance;
            
            // Draw outer circle
            ctx.beginPath();
            ctx.arc(x, y, 6, 0, 2 * Math.PI);
            ctx.fill();
            
            // Draw inner circle with value
            ctx.fillStyle = '#fff';
            ctx.beginPath();
            ctx.arc(x, y, 4, 0, 2 * Math.PI);
            ctx.fill();
            
            // Draw value text
            ctx.fillStyle = '#212529';
            ctx.font = 'bold 9px Arial';
            ctx.textAlign = 'center';
            ctx.textBaseline = 'middle';
            ctx.fillText(value.toFixed(1), x, y);
            
            // Reset point color for next iteration
            ctx.fillStyle = colors.point;
        }
    }
    
    console.log('Radar chart drawn successfully with', categories.length, 'categories');
};