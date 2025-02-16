import { useState, useEffect, useRef } from 'react';
import {
    Box,
    TextField,
    IconButton,
    List,
    ListItem,
    ListItemText,
    Paper,
    Typography,
} from '@mui/material';
import './App.css';
import SendIcon from '@mui/icons-material/Send';
import axios from 'axios';

function App() {
    const [messages, setMessages] = useState([]);
    const [input, setInput] = useState('');
    const [isThinking, setIsThinking] = useState(false);
    const messagesEndRef = useRef(null); // Ref oluştur

    // Scroll'u en aşağı kaydır
    const scrollToBottom = () => {
        messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
    };

    // Mesajlar güncellendiğinde scrollToBottom çağrılacak
    useEffect(() => {
        scrollToBottom();
    }, [messages]);

    const handleSendMessage = async () => {
        if (input.trim()) {
            setMessages(prevMessages => [
                ...prevMessages,
                { text: input, sender: 'You' },
                { text: 'Düşünüyor...', sender: 'Ev Asistanı' }
            ]);

            setIsThinking(true);
            setInput('');

            try {
                const response = await axios.post('https://localhost:7047/api/Objects/requestforhome', { messageRequest: input });
                console.log(response);
                setMessages(prevMessages => {
                    const updatedMessages = [...prevMessages];
                    updatedMessages[updatedMessages.length - 1] = {
                        text: response.data.messageResponse,
                        sender: 'Ev Asistanı'
                    };
                    return updatedMessages;
                });
            } catch (error) {
                console.error('Hata:', error);
            } finally {
                setIsThinking(false);
            }
        }
    };

    return (
        <Paper
            elevation={3}
            sx={{
                width: 800,
                height: 900,
                display: 'flex',
                flexDirection: 'column',
                p: 2,
            }}
        >
            <Typography variant='h6' sx={{ textAlign: 'center', mb: 1 }}>
                Ev Asistanı
            </Typography>
            <List sx={{ flexGrow: 1, overflowY: 'auto', maxHeight: 750 }}>
                {messages.map((msg, index) => (
                    <ListItem
                        key={index}
                        sx={{
                            justifyContent: msg.sender === 'You' ? 'flex-end' : 'flex-start',
                        }}
                    >
                        <ListItemText
                            primary={msg.text}
                            sx={{
                                backgroundColor: msg.sender === 'You' ? '#1976d2' : '#f5f5f5',
                                color: msg.sender === 'You' ? 'white' : 'black',
                                borderRadius: 2,
                                p: 1,
                                maxWidth: '75%',
                            }}
                        />
                    </ListItem>
                ))}
                <div ref={messagesEndRef} /> {/* Scroll'un ineceği nokta */}
            </List>
            <Box
                sx={{
                    display: 'flex',
                    alignItems: 'center',
                    mt: 'auto',
                    pt: 1,
                    borderTop: '1px solid #ccc',
                }}
            >
                <TextField
                    fullWidth
                    variant='outlined'
                    size='small'
                    value={input}
                    onChange={e => setInput(e.target.value)}
                    onKeyPress={e => e.key === 'Enter' && handleSendMessage()}
                    placeholder={isThinking ? 'Düşünüyor...' : 'Yardımcı olabileceğim bir konu varmıydı?'}
                    disabled={isThinking}
                />
                <IconButton color='primary' onClick={handleSendMessage} sx={{ ml: 1 }} disabled={isThinking}>
                    <SendIcon />
                </IconButton>
            </Box>
        </Paper>
    );
}

export default App;