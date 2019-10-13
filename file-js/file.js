var currentContents = '';
const displayElement = document.getElementById('contents');

document.getElementById('inputFile').addEventListener('change', (e) => {
  const file = e.target.files[0];

  const reader = new FileReader();
  reader.onload = (e) => {
    const fileContents = e.target.result;
    displayElement.textContent = fileContents;
    currentContents = fileContents;
  }
  reader.readAsText(file);
}, false);

document.getElementById('outputFile').addEventListener('click', (e) => {
  const downloadLink = document.createElement('a');
  downloadLink.download = 'export.txt';
  downloadLink.href = URL.createObjectURL(new Blob([currentContents], { 'type' : 'text/plain' }));
  //downloadLink.dataset.downloadurl = ['text/plain', downloadLink.download, downloadLink.href].join(':');
  downloadLink.click();
}, false);
