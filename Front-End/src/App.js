import React from 'react';
import UrlShortener from './components/UrlShortener';

function App() {
  return (
    <div className="App">
      <header style={styles.header}>
        <h1>Welcome to the URL Shortening Service</h1>
      </header>
      <main style={styles.main}>
        <UrlShortener />
      </main>
      <footer style={styles.footer}>
        <p>&copy; {new Date().getFullYear()} URL Shortener Service</p>
      </footer>
    </div>
  );
}

const styles = {
  header: {
    backgroundColor: 'rgba(13, 110, 253, 0.25)',
    padding: '20px',
    color: 'black',
    textAlign: 'center',
  },
  main: {
    padding: '20px',
  },
  footer: {
    backgroundColor: '#f1f1f1',
    padding: '10px',
    textAlign: 'center',
    position: 'fixed',
    width: '100%',
    bottom: '0',
  },
};

export default App;