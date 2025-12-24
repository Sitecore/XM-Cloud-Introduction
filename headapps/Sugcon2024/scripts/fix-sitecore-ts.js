#!/usr/bin/env node
/**
 * Adds // @ts-nocheck to auto-generated .sitecore/*.ts files
 * Handles 'use client' directives by placing @ts-nocheck after them
 */
const fs = require('fs');
const path = require('path');

const sitecoreDir = path.join(__dirname, '..', '.sitecore');

if (!fs.existsSync(sitecoreDir)) {
  console.log('.sitecore directory not found, skipping...');
  process.exit(0);
}

const files = fs.readdirSync(sitecoreDir).filter(f => f.endsWith('.ts'));

files.forEach(file => {
  const filePath = path.join(sitecoreDir, file);
  let content = fs.readFileSync(filePath, 'utf8');
  
  // Skip if already has @ts-nocheck
  if (content.includes('@ts-nocheck')) {
    console.log(`${file}: already has @ts-nocheck`);
    return;
  }
  
  // Put @ts-nocheck at the very top (before 'use client' if present)
  content = '// @ts-nocheck\n' + content;
  
  fs.writeFileSync(filePath, content);
  console.log(`${file}: added @ts-nocheck`);
});

console.log('Done fixing .sitecore TypeScript files');

