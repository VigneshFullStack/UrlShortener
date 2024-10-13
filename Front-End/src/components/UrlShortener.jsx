import React, { useState } from 'react';
import axios from 'axios';

const UrlShortener = () => {
  const [longUrl, setLongUrl] = useState('');
  const [shortUrl, setShortUrl] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false); 

  const handleShorten = async () => {
    try {
      setError('');
      setShortUrl(''); 
      setLoading(true); 
      
      const response = await axios.post(`${process.env.REACT_APP_API_BASE_URL}/UrlShortener/shorten`, { longUrl });
      setShortUrl(response.data.shortUrl);
      console.log("shortUrl : ", response.data.shortUrl);
    } catch (err) {
      setError('Failed to shorten URL.');
      console.error(err);
    } finally {
      setLoading(false); 
    }
  };

  return (
    <div style={styles.container}>
      <h2>URL Shortener</h2>
      <input
        type="text"
        placeholder="Enter long URL"
        value={longUrl}
        onChange={(e) => setLongUrl(e.target.value)}
        style={styles.input}
      />
      <button onClick={handleShorten} style={styles.button} disabled={loading}>
        {loading ? 'Shortening...' : 'Shorten'}
      </button>
      {shortUrl && (
        <div style={styles.result}>
          <p>
            Short URL: <a href={`${process.env.REACT_APP_API_BASE_URL}/UrlShortener/${shortUrl}`} target="_blank" rel="noopener noreferrer">{shortUrl}</a>
          </p>
        </div>
      )}
      {error && <p style={styles.error}>{error}</p>}
    </div>
  );
};

const styles = {
  container: {
    margin: '50px auto',
    padding: '20px',
    maxWidth: '500px',
    textAlign: 'center',
    border: '1px solid #ccc',
    borderRadius: '10px',
    boxShadow: '2px 2px 12px #aaa',
  },
  input: {
    width: '80%',
    padding: '10px',
    marginBottom: '10px',
    borderRadius: '5px',
    border: '1px solid #ddd',
  },
  button: {
    padding: '10px 20px',
    borderRadius: '5px',
    border: 'none',
    backgroundColor: '#28a745',
    color: '#fff',
    cursor: 'pointer',
  },
  result: {
    marginTop: '20px',
  },
  error: {
    color: 'red',
    marginTop: '10px',
  },
};

export default UrlShortener;
