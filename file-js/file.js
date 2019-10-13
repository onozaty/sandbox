let currentContents = '';
const displayElement = document.getElementById('contents');

document.getElementById('inputFile').addEventListener('click', (e) => {
  const fileInput = document.createElement('input');
  fileInput.type = 'file';
  fileInput.setAttribute('hidden', true);

  fileInput.addEventListener('change', (e) => {
    const file = e.target.files[0];
  
    const reader = new FileReader();
    reader.onload = (e) => {
      const fileContents = e.target.result;
      displayElement.textContent = fileContents;
      currentContents = fileContents;
    }
    reader.readAsText(file);
  }, false);
  
  document.body.appendChild(fileInput);
  fileInput.click();
  fileInput.remove();
}, false);


document.getElementById('outputFile').addEventListener('click', (e) => {
  const downloadLink = document.createElement('a');
  downloadLink.download = 'export.txt';
  downloadLink.href = URL.createObjectURL(new Blob([currentContents], { 'type' : 'text/plain' }));
  downloadLink.setAttribute('hidden', true);

  document.body.appendChild(downloadLink);
  downloadLink.click();
  downloadLink.remove();
}, false);
