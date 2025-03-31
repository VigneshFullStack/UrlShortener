import React, { useState, useEffect } from 'react';
import axios from 'axios';
import 'bootstrap/dist/css/bootstrap.min.css';

const UrlShortener = () => {
  const [longUrl, setLongUrl] = useState('');
  const [shortUrl, setShortUrl] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const [urlList, setUrlList] = useState([]);
  const [tableLoading, setTableLoading] = useState(true);

  useEffect(() => {
    fetchAllUrls();

    const handleVisibilityChange = () => {
      if (!document.hidden) {
        fetchAllUrls();
      }
    };
    document.addEventListener("visibilitychange", handleVisibilityChange);

    return () => {
      document.removeEventListener("visibilitychange", handleVisibilityChange);
    };
  }, []);

  const fetchAllUrls = async () => {
    setTableLoading(true);
    try {
      const response = await axios.get(`${process.env.REACT_APP_API_BASE_URL}/UrlShortener/all`);
      setUrlList(response.data);
    } catch (err) {
      console.error('Error fetching URLs:', err);
    } finally {
      setTableLoading(false);
    }
  };

  const handleShorten = async () => {
    try {
      setError('');
      setShortUrl('');
      setLoading(true);

      const response = await axios.post(`${process.env.REACT_APP_API_BASE_URL}/UrlShortener/shorten`, { longUrl });
      setShortUrl(response.data.shortUrl);
      fetchAllUrls();
    } catch (err) {
      setError('Failed to shorten URL.');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleClear = () => {
    setLongUrl('');
    setShortUrl('');
    setError('');
  };

  return (
    <div className="container mt-4 mb-5 p-4 border rounded shadow text-center">
      <h2>URL Shortener</h2>
      <input
        type="text"
        className="form-control my-3 mx-auto"
        style={{ maxWidth: '500px' }}
        placeholder="Enter long URL"
        value={longUrl}
        onChange={(e) => setLongUrl(e.target.value)}
      />

      <div className="mb-3">
        <button onClick={handleShorten} className="btn btn-success me-2" disabled={loading}>
          {loading ? 'Shortening...' : 'Shorten'}
        </button>
        <button onClick={handleClear} className="btn btn-danger">Clear</button>
      </div>

      {shortUrl && (
        <div className="alert alert-info" role="alert">
          Short URL:{' '}
          <a href={`${process.env.REACT_APP_API_BASE_URL}/UrlShortener/${shortUrl}`} target="_blank" rel="noopener noreferrer">
            {shortUrl}
          </a>
        </div>
      )}
      {error && <p className="text-danger">{error}</p>}

      <h3 className='mt-4'>Shortened URLs</h3>
      {tableLoading ? (
        <div className="text-center my-3">
          <div className="spinner-border text-primary" role="status">
            <span className="visually-hidden">Loading...</span>
          </div>
        </div>
      ) : (
        <table className="table table-bordered table-striped mt-3">
          <thead className="table-dark">
            <tr>
              <th>Short URL</th>
              <th>Long URL</th>
              <th>Access Count</th>
            </tr>
          </thead>
          <tbody>
            {urlList.map((url) => (
              <tr key={url.shortUrl}>
                <td>
                  <a
                    href={`${process.env.REACT_APP_API_BASE_URL}/UrlShortener/${url.shortUrl}`}
                    target="_blank"
                    rel="noopener noreferrer"
                  >
                    {url.shortUrl}
                  </a>
                </td>
                <td>{url.longUrl}</td>
                <td>{url.accessCount}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default UrlShortener;